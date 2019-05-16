using DataAccess;
using Domain.Domain;
using Domain.Domain.JournalItems;
using Domain.RequestModels.Journal;
using Domain.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service
{
    public class JournalService : Service, IJournalService
    {
        public JournalService(DbContextOptions<DndContext> options) : base(options)
        {
        }

        public async Task<JournalItem> AddJournalItemToGameAsync(AddJournalItemModel model, Guid gameId, Guid userId)
        {
            var game = await GetGameByIdWithOwner(gameId);

            var result = await game.AddJournalItemAsync(model, userId);

            await context.SaveChangesAsync();

            return result;
        }

        public async Task<ICollection<JournalItem>> GetAllJournalItemsAsync(Guid userId, Guid gameId)
        {
            var game = await GetGameByIdWithOwner(gameId);
            var isOwner = game.IsOwner(userId);

            var journalItems = game.JournalItems.Where(j => j.Type == JournalItemType.Folder || j.Permissions.Any(p => p.UserId == userId && p.CanSee == true));

            if (!isOwner)
            {
                FilterEmptyFolders(journalItems);
            }

            return journalItems.ToList();
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

        private async Task<Game> GetGameByIdWithOwner(Guid gameId)
        {
            return await context.Games.Include(g => g.Owner).FilterByGameId(gameId).FirstOrDefaultAsync();
        }
    }
}
