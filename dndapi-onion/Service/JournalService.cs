using DataAccess;
using Domain.Domain;
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
using AutoMapper.QueryableExtensions;
using Domain;

namespace Service
{
    public class JournalService : IJournalService
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public JournalService(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<JournalItemTreeItemDto> AddJournalItemToGame(AddJournalItemDto dto, Guid gameId)
        {
            if (dto.ParentFolderId == null)
            {
                var game = await repository.Games.FilterById(gameId).FirstOrDefaultAsync();

                var journalItem = await game.AddJournalItemAsync(dto);

                await repository.Commit();

                return mapper.Map<JournalItem, JournalItemTreeItemDto>(journalItem);
            }

            var parent = await repository.JournalFolders.FilterById(dto.ParentFolderId.Value).FirstOrDefaultAsync();

            var result = await parent.AddJournalItem(dto, gameId);

            await repository.Commit();

            return mapper.Map<JournalItem, JournalItemTreeItemDto>(result); ;
        }

        public async Task<IEnumerable<JournalItemTreeItemDto>> GetJournalItemsForParentFolderId(Guid userId, Guid gameId, Guid? parentFolderId)
        {
            var isOwner = await repository.Games.FilterOnOwnerById(userId).AnyAsync();
            if (isOwner)
            {
                return await GetJournalItemsForParentFolderIdWithEmptyFolders(gameId, parentFolderId);
            }

            var maxDepth = await repository.JournalItems.FilterByGameId(gameId).MaxAsync(g => g.Level);
           // var query = repository.JournalItems.FilterByParentFolderId(parentFolderId);
            var folderQuery = repository.JournalFolders.Include(j => j.JournalItems).FilterByParentFolderId(parentFolderId);

            //for (int q = 0; q < maxDepth; q++)
            //{
            //    //query = query.Where(j => j.Type != JournalItemType.Folder);
            //    query = query
            //        .Where(j => j.Type == JournalItemType.Folder && ((JournalFolder)j).JournalItems.Any(i =>
            //                       (i.Type == JournalItemType.Folder &&
            //                        ((JournalFolder)i).JournalItems.Any(d => d.Type != JournalItemType.Folder)) ||
            //                       i.Type != JournalItemType.Folder));

            //}


            //var test = repository.JournalFolders.Include(j => j.JournalItems).FilterByGameId(gameId).FilterByParentFolderId(parentFolderId);


            for (int q = 0; q < maxDepth; q++)
            {
                folderQuery = folderQuery.Where(j => j.Type == JournalItemType.Folder).Select(s => s).Where(ji => ji.JournalItems.Any(di => di.Type != JournalItemType.Folder));

                //folderQuery = folderQuery.Where(j => j.JournalItems.Where(ji => ji.Type == JournalItemType.Folder).Select(a => a).Any(i =>
                //    i.Type == JournalItemType.Folder && (i as JournalFolder).JournalItems.Any()
                //    ));
            }

            var otherone = repository.JournalItems.FilterByParentFolderId(parentFolderId).Where(j => j.Type != JournalItemType.Folder);

            var folders = await folderQuery.Select(j => new JournalItemTreeItemDto
            {
                Id = j.Id,
                ParentFolderId = j.ParentFolderId,
                Name = j.Name,
                ImageId = j.ImageId,
                Type = j.Type
            }).ToListAsync();

            var items = await  otherone.Select(j => new JournalItemTreeItemDto
            {
                Id = j.Id,
                ParentFolderId = j.ParentFolderId,
                Name = j.Name,
                ImageId = j.ImageId,
                Type = j.Type
            }).ToListAsync();

            folders.AddRange(items);

            return folders;
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

        private void FilterEmptyFolders(IEnumerable<JournalItem> journalItems)
        {
            var folders = journalItems.Where(j => j.Type == JournalItemType.Folder).Select(j => j as JournalFolder).ToList();

            foreach (var folder in folders)
            {
                FilterEmptyFoldersImpl(journalItems.ToList(), folder);
            }
        }

        private void FilterEmptyFoldersImpl(ICollection<JournalItem> root, JournalFolder folder, JournalFolder parent = null)
        {
            var folders = folder.JournalItems.Where(j => j.Type == JournalItemType.Folder).Select(j => j as JournalFolder).ToList();

            foreach (var child in folders)
            {
                FilterEmptyFoldersImpl(root, child, folder);
            }

            if (folder.JournalItems.Count() < 1)
            {
                if (parent == null)
                {
                    root.Remove(folder);
                }
                else
                {
                    parent.JournalItems.Remove(folder);
                }
            }
        }

    }
}
