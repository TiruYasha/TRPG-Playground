using Domain.Domain.JournalItems;
using Domain.RequestModels.Journal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.ServiceInterfaces
{
    public interface IJournalService
    {
        Task<JournalItem> AddJournalItemToGameAsync(AddJournalItemModel model, Guid gameId, Guid userId);
        Task<ICollection<JournalItem>> GetAllJournalItemsAsync(Guid userId, Guid gameId);
    }
}
