using System.Collections.Generic;

namespace Domain.ReturnModels.Journal.JournalItems
{
    public class JournalFolderModel : JournalItemModel
    {
        public virtual ICollection<JournalItemModel> JournalItems { get; set; }
    }
}
