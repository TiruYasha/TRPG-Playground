using DataAccess;
using Domain.Domain.JournalItems;
using Domain.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Config;
using Domain.Dto.RequestDto.Journal;
using Domain.Dto.ReturnDto.Journal;
using Domain.Dto.Shared;
using Domain.Exceptions;
using Domain.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Service.Utilities;

namespace Service
{
    public class JournalService : IJournalService
    {
        private readonly IMapper mapper;
        private readonly FileStorageConfig fileStorageConfig;
        private readonly ImageProcesser imageProcessor;
        private readonly DndContext context;

        public JournalService(DndContext context, IMapper mapper, IOptions<FileStorageConfig> fileStorageConfig, ImageProcesser imageProcessor)
        {
            this.mapper = mapper;
            this.fileStorageConfig = fileStorageConfig.Value;
            this.imageProcessor = imageProcessor;
            this.context = context;
        }

        public async Task<(JournalItemTreeItemDto, List<Guid>)> AddJournalItemToGame(AddJournalItemDto dto, Guid gameId)
        {
            // TODO  fix canEdit boolean
            if (dto.ParentFolderId == null)
            {
                var game = await context.Games.FilterById(gameId).FirstOrDefaultAsync();

                var journalItem = await game.AddJournalItem(dto.JournalItem);

                await context.SaveChangesAsync();

                return GetJournalItemTreeItemWithCanSeePermissions(journalItem);
            }

            var parent = await context.JournalFolders.FilterById(dto.ParentFolderId.Value).FirstOrDefaultAsync();

            var result = await parent.AddJournalItem(dto.JournalItem, gameId);

            await context.SaveChangesAsync();

            return GetJournalItemTreeItemWithCanSeePermissions(result);
        }

        public async Task<JournalItemTreeItemDto> UpdateJournalItem(JournalItemDto dto, Guid gameId, Guid userId)
        {
            //TODO get canedit/cansee in controller
            var journalItem = await context.JournalItems.FilterById(dto.Id).FilterByGameId(gameId).FilterByCanEdit(userId).FirstOrDefaultAsync();

            if (journalItem == null)
            {
                throw new PermissionException("You do not have permission or the journalitem could not be found");
            }

            await journalItem.Update(dto);
            await context.SaveChangesAsync();

            return mapper.Map<JournalItem, JournalItemTreeItemDto>(journalItem);
        }

        public async Task<IEnumerable<JournalItemTreeItemDto>> GetJournalItemsForParentFolderId(Guid userId, Guid gameId, Guid? parentFolderId)
        {
            var isOwner = await context.Games.FilterOnOwnerById(userId).AnyAsync();
            if (isOwner)
            {
                return await GetJournalItemsForParentFolderIdWithEmptyFolders(gameId, parentFolderId);
            }

            var query = context.JournalItems.FilterByParentFolderId(parentFolderId).Where(i => i.Type == JournalItemType.Folder || (i.Permissions.Any(p => p.UserId == userId && p.CanSee || p.CanEdit)))
                .Select(j => new {j.Type, j.Name, j.ParentFolderId, j.Id, j.ImageId, CanEdit = j.Permissions.Any(p => p.UserId == userId && p.CanEdit)});

            return await query.ProjectTo<JournalItemTreeItemDto>(mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<Guid> UploadImage(IFormFile file, Guid gameId, Guid journalItemId)
        {
            //TODO: In future updates also limit size of images.
            var originalName = file.FileName;
            var extension = Path.GetExtension(originalName);

            if (!FileExtensionValidator.ValidateImageExtension(extension))
            {
                throw new BadImageFormatException($"The format {extension} is not supported");
            }

            var journalItem = context.JournalItems.FirstOrDefault(j => j.Id == journalItemId);
            var image = await journalItem.SetImage(extension, originalName);

            await imageProcessor.SaveImage(file, fileStorageConfig.BigImageLocation + gameId, $"{image.Id}{extension}");

            await context.SaveChangesAsync();

            return image.Id;
        }

        public async Task<byte[]> GetImage(Guid journalItemId, bool isThumbnail)
        {
            var location = isThumbnail ? fileStorageConfig.ThumbnailLocation : fileStorageConfig.BigImageLocation;
            var image = await context.JournalItems.Include(j => j.Image).FilterById(journalItemId).Select(j => new { j.GameId, j.ImageId, j.Image.Extension }).FirstOrDefaultAsync();

            if (image.ImageId == null) return new byte[0];

            var fullPath = $"{location}{image.GameId}/{image.ImageId}{image.Extension}";

            var binary = await File.ReadAllBytesAsync(fullPath);

            return binary;
        }

        public async Task<JournalItemDto> GetJournalItemById(Guid userId, Guid journalItemId)
        {
            var journalItem = await context.JournalItems.Include(j => j.Permissions).FilterById(journalItemId).FilterByCanSee(userId).FirstOrDefaultAsync();

            if (journalItem == null)
            {
                throw new PermissionException("Access Denied");
            }

            return mapper.Map<JournalItem, JournalItemDto>(journalItem);
        }

        public async Task<IEnumerable<JournalItemPermission>> GetJournalItemPermissions(Guid journalItemId)
        {
            return await context.JournalItems.Include(j => j.Permissions).FilterById(journalItemId)
                .SelectMany(j => j.Permissions).ToListAsync();
        }

        public async Task DeleteJournalItem(Guid journalItemId)
        {
            var item = await context.JournalItems.FilterById(journalItemId).FirstOrDefaultAsync();
            context.JournalItems.Remove(item);
            await context.SaveChangesAsync();
        }

        private (JournalItemTreeItemDto, List<Guid>) GetJournalItemTreeItemWithCanSeePermissions(JournalItem journalItem)
        {
            return (mapper.Map<JournalItem, JournalItemTreeItemDto>(journalItem), journalItem.Permissions.Where(w => w.CanSee == true).Select(s => s.UserId).ToList());
        }

        private async Task<IEnumerable<JournalItemTreeItemDto>> GetJournalItemsForParentFolderIdWithEmptyFolders(Guid gameId, Guid? parentFolderId)
        {
            var query = context.JournalItems.FilterByParentFolderId(parentFolderId);

            if (!parentFolderId.HasValue)
            {
                query = query.FilterByGameId(gameId);
            }

            var result = await query.ProjectTo<JournalItemTreeItemDto>(mapper.ConfigurationProvider).ToListAsync();

            return result;
        }
    }
}
