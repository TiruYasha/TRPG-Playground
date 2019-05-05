using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RequestModels.Journal.JournalItems
{
    public class JournalHandoutModel : JournalItemModel
    {
        public string Description { get; set; }
        public string OwnerNotes { get; set; }
        public JournalHandoutModel()
        {
            Type = Domain.JournalItems.JournalItemType.Handout;
        }
    }
}
