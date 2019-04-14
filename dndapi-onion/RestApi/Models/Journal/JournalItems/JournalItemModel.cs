using Domain.Domain.JournalItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestApi.Models.Journal.JournalItems
{
    public class JournalItemModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public JournalItemType Type { get; set; }
    }
}
