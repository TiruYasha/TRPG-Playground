using Domain.Domain.JournalItems;
using Domain.RequestModels.Journal;
using Domain.ReturnModels.Journal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.ServiceInterfaces
{
    public interface IJournalService
    {
        Task<JournalItem> AddJournalItemToGameAsync(AddJournalItemModel model, Guid gameId);
        Task<ICollection<JournalItem>> GetAllJournalItemsAsync(Guid userId, Guid gameId);

        Task<IEnumerable<AddedJournalItemModel>> GetJournalItemsForParentFolderId(Guid userId, Guid gameId, Guid parentFolderId);
    }
}
