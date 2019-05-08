using Domain.RequestModels.Journal;
using Domain.RequestModels.Journal.JournalItems;
using System;

namespace Domain.Domain.JournalItems
{
    public class JournalHandout : JournalItem
    {
        public string Description { get; set; }
        public string OwnerNotes { get; set; }

        public JournalHandout() : base()
        {

        }

        public JournalHandout(AddJournalItemModel model, Guid gameId) : base(JournalItemType.Handout, model.JournalItem.Name, gameId, null, model.JournalItem.CanSee, model.JournalItem.CanEdit)
        {
            var handoutModel = model.JournalItem as JournalHandoutModel;
            Description = handoutModel.Description;
            OwnerNotes = handoutModel.OwnerNotes;
        }
    }
}
