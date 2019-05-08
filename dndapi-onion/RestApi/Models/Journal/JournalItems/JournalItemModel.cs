using Domain.Domain.JournalItems;
using System;

namespace RestApi.Models.Journal.JournalItems
{
    public class JournalItemModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public JournalItemType Type { get; set; }
    }
}
