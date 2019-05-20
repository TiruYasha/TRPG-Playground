using Domain.RequestModels.Journal.JournalItems;
using System;

namespace Domain.RequestModels.Journal
{
    public class AddJournalItemDto
    {
        public JournalItemDto JournalItem { get; set; }
        public Guid? ParentFolderId { get; set; }
    }
}
