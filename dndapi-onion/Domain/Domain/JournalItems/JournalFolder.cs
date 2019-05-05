using Domain.Exceptions;
using Domain.RequestModels.Journal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Domain.JournalItems
{
    public class JournalFolder : JournalItem
    {
        public virtual ICollection<JournalItem> JournalItems { get; set; }
        public JournalFolder() : base(JournalItemType.Folder, "t", null, null, null)
        {
            JournalItems = new List<JournalItem>();
        }

        public JournalFolder(AddJournalItemModel model) : base(JournalItemType.Folder, model.Name, null, null, null)
        {
            JournalItems = new List<JournalItem>();
        }
     
        public void AddJournalItem(JournalItem item)
        {
            if(item == null)
            {
                throw new ArgumentNullException("item");
            }

            JournalItems.Add(item);
        }
    }
}
