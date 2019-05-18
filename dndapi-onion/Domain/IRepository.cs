using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Domain;
using Domain.Domain.JournalItems;

namespace Domain
{
    public interface IRepository
    {
        IQueryable<Game> Games { get; }
        IQueryable<GamePlayer> GamePlayers { get; }
        IQueryable<ChatMessage> ChatMessages { get; }
        IQueryable<JournalItem> JournalItems { get; }
        IQueryable<JournalFolder> JournalFolders { get; }
        Task Commit();
    }
}
