using System;
using System.Collections.Generic;
using Domain.Domain.JournalItems;
using Domain.JsonConverter;
using Newtonsoft.Json;

namespace Domain.Dto.Shared
{
    [JsonConverter(typeof(JournalItemModelConverter))]
    public class JournalItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ImageId { get; set; }
        public JournalItemType Type { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastEditedOn { get; set; }
        public List<JournalItemPermissionDto> Permissions { get; set; }
        public JournalItemDto()
        {
            Permissions = new List<JournalItemPermissionDto>();
        }
    }
}
