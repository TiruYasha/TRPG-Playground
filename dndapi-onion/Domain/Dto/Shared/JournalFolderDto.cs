using System.Collections.Generic;

namespace Domain.Dto.Shared
{
    public class JournalFolderDto: JournalItemDto
    {
        public virtual ICollection<JournalItemDto> JournalItems { get; set; }

        public JournalFolderDto()
        {
            Type = Domain.JournalItems.JournalItemType.Folder;
        }
    }
}
