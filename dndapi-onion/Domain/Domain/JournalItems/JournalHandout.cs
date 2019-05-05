using Domain.RequestModels.Journal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Domain.JournalItems
{
    public class JournalHandout : JournalItem
    {
        public string Description { get; set; }
        public string OwnerNotes { get; set; }
        public JournalHandout(AddJournalItemModel model) : base(JournalItemType.Handout, model.JournalItem.Name, null, null,null)
        {

        }
    }
}
