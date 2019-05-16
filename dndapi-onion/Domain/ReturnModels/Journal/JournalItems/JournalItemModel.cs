using System;
using Domain.Domain.JournalItems;

namespace Domain.ReturnModels.Journal.JournalItems
{
    public class JournalItemModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public JournalItemType Type { get; set; }
    }
}
