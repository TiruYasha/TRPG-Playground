using Domain.RequestModels.Journal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Domain.JournalItems
{
    public class JournalFolder : JournalItem
    {
        public virtual IList<JournalItem> JournalItems { get; set; }

        private JournalFolder()
        {
            JournalItems = new List<JournalItem>();
        }

        public JournalFolder(AddJournalItemDto dto, Guid gameId) : base(JournalItemType.Folder, dto.JournalItem.Name, gameId, null, null)
        {
            JournalItems = new List<JournalItem>();
        }

        public virtual Task<JournalItem> AddJournalItem(AddJournalItemDto dto, Guid gameId)
        {
            return Task.Run(() =>
            {
                if (dto?.JournalItem == null)
                {
                    throw new ArgumentNullException(nameof(dto));
                }

                var journalItem = JournalItemFactory.Create(dto, gameId);

                JournalItems.Add(journalItem);
                return journalItem;
            });
        }
    }
}
