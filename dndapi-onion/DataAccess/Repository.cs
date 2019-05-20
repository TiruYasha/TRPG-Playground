using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Domain;
using Domain.Domain.JournalItems;

namespace DataAccess
{
    public class Repository : IRepository
    {
        private readonly DndContext context;

        public Repository(DndContext context)
        {
            this.context = context;
        }

        public IQueryable<Game> Games => context.Games;

        public IQueryable<GamePlayer> GamePlayers => context.GamePlayers;

        public IQueryable<ChatMessage> ChatMessages => context.ChatMessages;
        public IQueryable<JournalItem> JournalItems => context.JournalItems;
        public IQueryable<JournalFolder> JournalFolders => context.JournalFolders;
        public Task Commit()
        {
            return context.SaveChangesAsync();
        }
    }
}
