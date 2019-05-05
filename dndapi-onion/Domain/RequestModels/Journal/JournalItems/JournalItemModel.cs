using Domain.Domain.JournalItems;
using Domain.JsonConverter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Domain.RequestModels.Journal.JournalItems
{
    [JsonConverter(typeof(JournalItemModelConverter))]
    public class JournalItemModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageId { get; set; }
        public JournalItemType Type { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastEditedOn { get; set; }
        public ICollection<Guid> CanSee {get;set;}
        public ICollection<Guid> CanEdit { get; set; }

        public JournalItemModel() { }
    }
}
