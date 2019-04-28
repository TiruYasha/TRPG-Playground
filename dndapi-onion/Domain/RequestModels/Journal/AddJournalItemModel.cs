using Domain.Domain;
using Domain.Domain.JournalItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RequestModels.Journal
{
    public class AddJournalItemModel
    {
        public string Name { get; set; }
        public Guid ParentFolderId { get; set; }
        public JournalItemType JournalItemType { get; set; }
        public IList<Guid> CanSee { get; set; }
        public IList<Guid> CanEdit { get; set; }
    }
}
