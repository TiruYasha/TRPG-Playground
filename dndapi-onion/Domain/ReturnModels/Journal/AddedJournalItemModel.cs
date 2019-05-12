using System;
using Domain.Domain.JournalItems;

namespace Domain.ReturnModels.Journal
{
    public class AddedJournalItemModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ParentId { get; set; }
        public JournalItemType Type { get; set; }
        public Guid ImageId { get; set; }
    }
}
