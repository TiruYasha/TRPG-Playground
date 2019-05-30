using Domain.Domain.JournalItems;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Domain.Dto.RequestDto.Journal;
using Domain.Dto.ReturnDto.Journal;
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
