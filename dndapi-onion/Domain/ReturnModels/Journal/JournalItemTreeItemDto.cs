using System;
using Domain.Domain.JournalItems;

namespace Domain.ReturnModels.Journal
{
    public class JournalItemTreeItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentFolderId { get; set; }
        public JournalItemType Type { get; set; }
        public Guid? ImageId { get; set; }
    }
}
