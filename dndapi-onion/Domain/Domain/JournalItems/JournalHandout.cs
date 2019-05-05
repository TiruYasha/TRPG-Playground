using Domain.RequestModels.Journal;
using Domain.RequestModels.Journal.JournalItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Domain.JournalItems
{
    public class JournalHandout : JournalItem
    {
        public string Description { get; set; }
        public string OwnerNotes { get; set; }

        public JournalHandout() : base(JournalItemType.Handout, "t", null, null, null)
        {

        }

        public JournalHandout(AddJournalItemModel model) : base(JournalItemType.Handout, model.JournalItem.Name, null, null,null)
        {
            var handoutModel = model.JournalItem as JournalHandoutModel;
            Description = handoutModel.Description;
            OwnerNotes = handoutModel.OwnerNotes;
        }
    }
}
