using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.JournalItems;

namespace Logic
{
    public interface IJournalLogic
    {
        Task AddJournalItemToGameAsync(Guid id, JournalItemModel journalItem);
        Task<List<JournalItemModel>> GetAllJournalItemsAsync(Guid gameId);
    }
}