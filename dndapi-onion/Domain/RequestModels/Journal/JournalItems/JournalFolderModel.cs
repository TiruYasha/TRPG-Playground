using System.Collections.Generic;

namespace Domain.RequestModels.Journal.JournalItems
{
    public class JournalFolderModel: JournalItemModel
    {
        public virtual ICollection<JournalItemModel> JournalItems { get; set; }

        public JournalFolderModel()
        {
            Type = Domain.JournalItems.JournalItemType.Folder;
        }
    }
}
