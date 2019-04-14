using System;
using System.Collections.Generic;
using System.Text;

namespace RestApi.Models.Journal.JournalItems
{
    public class JournalFolderModel : JournalItemModel
    {
        public virtual ICollection<JournalItemModel> JournalItems { get; set; }
    }
}
