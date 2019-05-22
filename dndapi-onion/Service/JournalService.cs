﻿using DataAccess;
using Domain.Domain.JournalItems;
using Domain.RequestModels.Journal;
using Domain.ReturnModels.Journal;
using Domain.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Domain.Config;
using Domain.Domain;
using Domain.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Service
{
    public class JournalService : IJournalService
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;
        private readonly FileStorageConfig fileStorageConfig;
        private readonly ImageProcesser processer;

        public JournalService(IRepository repository, IMapper mapper, IOptions<FileStorageConfig> fileStorageConfig, ImageProcesser processer)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.fileStorageConfig = fileStorageConfig.Value;
            this.processer = processer;
        }

        public async Task<(JournalItemTreeItemDto, List<Guid>)> AddJournalItemToGame(AddJournalItemDto dto, Guid gameId)
        {
            if (dto.ParentFolderId == null)
            {
                var game = await repository.Games.FilterById(gameId).FirstOrDefaultAsync();

                var journalItem = await game.AddJournalItemAsync(dto);

                await repository.Commit();

                return GetJournalItemTreeItemWithCanSeePermissions(journalItem);
            }

            var parent = await repository.JournalFolders.FilterById(dto.ParentFolderId.Value).FirstOrDefaultAsync();

            var result = await parent.AddJournalItem(dto, gameId);

            await repository.Commit();

            return GetJournalItemTreeItemWithCanSeePermissions(result);
        }

        public async Task<IEnumerable<JournalItemTreeItemDto>> GetJournalItemsForParentFolderId(Guid userId, Guid gameId, Guid? parentFolderId)
        {
            var isOwner = await repository.Games.FilterOnOwnerById(userId).AnyAsync();
            if (isOwner)
            {
                return await GetJournalItemsForParentFolderIdWithEmptyFolders(gameId, parentFolderId);
            }

            var query = repository.JournalItems.FilterByParentFolderId(parentFolderId).Where(i => i.Type == JournalItemType.Folder || (i.Permissions.Any(p => p.UserId == userId && p.CanSee || p.CanEdit)));

            return await query.Select(j => new JournalItemTreeItemDto
            {
                Id = j.Id,
                ParentFolderId = j.ParentFolderId,
                Name = j.Name,
                ImageId = j.ImageId,
                Type = j.Type
            }).ToListAsync();
        }

        public async Task<Guid> UploadImage(IFormFile file, Guid gameId, Guid journalItemId)
        {
            var originalName = file.FileName;

            var image = new Image(originalName);
            var extension = System.IO.Path.GetExtension(originalName);

            await processer.SaveImage(file, fileStorageConfig.BigImageLocation + gameId, $"{image.Id}.{extension}");

            var journalItem = repository.JournalItems.FirstOrDefault(j => j.Id == journalItemId && j.GameId == gameId);
            journalItem.ImageId = image.Id;

            await repository.Commit();

            return image.Id;
        }

        private (JournalItemTreeItemDto, List<Guid>) GetJournalItemTreeItemWithCanSeePermissions(JournalItem journalItem)
        {
            return (mapper.Map<JournalItem, JournalItemTreeItemDto>(journalItem), journalItem.Permissions.Where(w => w.CanSee == true).Select(s => s.UserId).ToList());
        }

        private async Task<IEnumerable<JournalItemTreeItemDto>> GetJournalItemsForParentFolderIdWithEmptyFolders(Guid gameId, Guid? parentFolderId)
        {
            var query = repository.JournalItems.FilterByParentFolderId(parentFolderId);

            if (!parentFolderId.HasValue)
            {
                query = query.FilterByGameId(gameId);
            }

            var result = await query.Select(j => new JournalItemTreeItemDto
            {
                Id = j.Id,
                ParentFolderId = j.ParentFolderId,
                Name = j.Name,
                ImageId = j.ImageId,
                Type = j.Type
            }).ToListAsync();

            return result;
        }
    }
}
