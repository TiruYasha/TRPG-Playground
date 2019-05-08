using System.Collections.Generic;

namespace RestApi.Models.Journal.JournalItems
{
    public class JournalFolderModel : JournalItemModel
    {
        public virtual ICollection<JournalItemModel> JournalItems { get; set; }
    }
}
