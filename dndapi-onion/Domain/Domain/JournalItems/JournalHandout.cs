using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Domain.JournalItems
{
    public class JournalHandout : JournalItem
    {
        public string Description { get; set; }
        public string OwnerNotes { get; set; }
        public JournalHandout() : base(JournalItemType.Handout, "")
        {

        }
    }
}
