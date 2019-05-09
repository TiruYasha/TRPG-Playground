using Domain.Domain.JournalItems;
using Domain.RepositoryInterfaces;
using Domain.RequestModels.Journal;
using Domain.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service
{
    public class JournalService : IJournalService
    {
        private readonly IGameRepository gameRepository;

        public JournalService(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }

        public async Task<JournalItem> AddJournalItemToGameAsync(AddJournalItemModel model, Guid gameId, Guid userId)
        {
            var game = await gameRepository.GetGameByIdAsync(gameId);

            var result = await game.AddJournalItemAsync(model, userId);

            await gameRepository.UpdateGameAsync(game);

            return result;
        }

        public async Task<ICollection<JournalItem>> GetAllJournalItemsAsync(Guid userId, Guid gameId)
        {
            var game = await gameRepository.GetGameByIdAsync(gameId);
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

        /*
         *  private static findChildImpl<ID, ITEM>(id: ID, root: ITEM, getChildren: getChildren, compare: compare): ITEM {
        const stack: ITEM[] = []
        let node: ITEM, ii;
        stack.push(root);

        while (stack.length > 0) {
            node = stack.pop();
            const children = getChildren(node);
            if (compare(id, node)) {
                return node;
            } else if (children && children.length) {
                for (ii = 0; ii < children.length; ii += 1) {
                    stack.push(children[ii]);
                }
            }
        }

         * */
    }
}
