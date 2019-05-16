using Domain.RequestModels.Journal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            JournalItems.Add(item);
        }

        public Task<JournalItem> AddJournalItemAsync(AddJournalItemModel model)
        {
            return Task.Run(() =>
            {
                if (model?.JournalItem == null)
                {
                    throw new ArgumentNullException("item");
                }

                var journalItem = JournalItemFactory.Create(model, Id);

                JournalItems.Add(journalItem);

                return journalItem;
            });
        }
    }
}
