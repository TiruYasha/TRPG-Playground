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
        Task<JournalItemTreeItemDto> AddJournalItemToGame(AddJournalItemDto dto, Guid gameId);
        Task<ICollection<JournalItem>> GetAllJournalItemsAsync(Guid userId, Guid gameId);

        Task<IEnumerable<JournalItemTreeItemDto>> GetJournalItemsForParentFolderId(Guid userId, Guid gameId, Guid parentFolderId);
    }
}
