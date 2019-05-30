using Domain.RequestModels.Journal;
using Domain.RequestModels.Journal.JournalItems;
using System;

namespace Domain.Domain.JournalItems
{
    public class JournalHandout : JournalItem
    {
        public string Description { get; set; }
        public string OwnerNotes { get; set; }

        private JournalHandout() { }

        public JournalHandout(AddJournalItemDto dto, Guid gameId) : base(JournalItemType.Handout, dto.JournalItem.Name, gameId, dto.JournalItem.CanSee, dto.JournalItem.CanEdit)
        {
            var handoutModel = dto.JournalItem as JournalHandoutDto;
            Description = handoutModel.Description;
            OwnerNotes = handoutModel.OwnerNotes;
        }
    }
}
