using Domain.RequestModels.Journal;
using System;
using System.Collections.Generic;

namespace Domain.Domain.JournalItems
{
    public class JournalFolder : JournalItem
    {
        public virtual ICollection<JournalItem> JournalItems { get; set; }
        public JournalFolder() : base()
        {
            JournalItems = new List<JournalItem>();
        }

        public JournalFolder(AddJournalItemModel model, Guid gameId) : base(JournalItemType.Folder, model.JournalItem.Name, gameId, null, null, null)
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
