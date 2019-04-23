using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Domain.JournalItems
{
    public class JournalFolder : JournalItem
    {
        public virtual ICollection<JournalItem> JournalItems { get; set; }
        public JournalFolder() : this("t")
        {
           
        }

        public JournalFolder(string name) : base(JournalItemType.Folder, name)
        {
            CheckArguments(name);

            Name = name;
            JournalItems = new List<JournalItem>();
        }

        private void CheckArguments(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new JournalItemException("The folder name is empty");
            }
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
