using System;
using Domain.Dto.Shared;

namespace Domain.Dto.RequestDto.Journal
{
    public class AddJournalItemDto
    {
        public JournalItemDto JournalItem { get; set; }
        public Guid? ParentFolderId { get; set; }
    }
}
