using Domain.Domain.JournalItems;
using Domain.RequestModels.Journal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ServiceInterfaces
{
    public interface IJournalService
    {
        Task<JournalFolder> AddJournalFolderToGameAsync(AddJournalFolderModel model, Guid userId);
        Task<ICollection<JournalItem>> GetAllJournalItemsAsync(Guid gameId);
    }
}
