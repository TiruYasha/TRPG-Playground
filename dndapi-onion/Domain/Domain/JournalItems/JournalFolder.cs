using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Dto.RequestDto.Journal;
using Domain.Dto.Shared;

namespace Domain.Domain.JournalItems
{
    public class JournalFolder : JournalItem
    {
        public virtual IList<JournalItem> JournalItems { get; set; }

        private JournalFolder()
        {
            JournalItems = new List<JournalItem>();
        }

        public JournalFolder(JournalItemDto dto, Guid gameId) : base(dto, gameId)
        {
            JournalItems = new List<JournalItem>();
        }

        public virtual Task<JournalItem> AddJournalItem(JournalItemDto dto, Guid gameId)
        {
            return Task.Run(() =>
            {
                if (dto == null)
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
