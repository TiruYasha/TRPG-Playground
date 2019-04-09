using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Domain.JournalItems
{
    public class JournalFolder : JournalItem
    {
        public ICollection<JournalItem> JournalItems { get; set; }
        public JournalFolder() : this("")
        {
           
        }

        public JournalFolder(string name) : base(JournalItemType.Folder, name)
        {
            Name = name;
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
