using Domain.Domain.JournalItems;
using Domain.RequestModels.Journal;
using Domain.ReturnModels.Journal;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Domain.ServiceInterfaces
{
    public interface IJournalService
    {
        Task<(JournalItemTreeItemDto, List<Guid>)> AddJournalItemToGame(AddJournalItemDto dto, Guid gameId);

        Task<IEnumerable<JournalItemTreeItemDto>> GetJournalItemsForParentFolderId(Guid userId, Guid gameId, Guid? parentFolderId);

        Task<Guid> UploadImage(IFormFile file, Guid gameId, Guid journalItemId);
        Task<byte[]> GetImage(Guid journalItemId, bool isThumbnail);
    }
}
