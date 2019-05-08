using Domain.RequestModels.Journal.JournalItems;
using System;

namespace Domain.RequestModels.Journal
{
    public class AddJournalItemModel
    {
        public JournalItemModel JournalItem { get; set; }
        public Guid ParentFolderId { get; set; }
    }
}
