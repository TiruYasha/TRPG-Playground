using Domain.Domain;
using Domain.Domain.JournalItems;
using Domain.RequestModels.Journal.JournalItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RequestModels.Journal
{
    public class AddJournalItemModel
    {
        public JournalItemModel JournalItem { get; set; }
        public Guid ParentFolderId { get; set; }
    }
}
