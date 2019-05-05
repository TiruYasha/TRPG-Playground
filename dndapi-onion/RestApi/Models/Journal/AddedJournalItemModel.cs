using Domain.Domain.JournalItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestApi.Models.Journal
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
