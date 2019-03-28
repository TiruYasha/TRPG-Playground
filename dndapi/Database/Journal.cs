using Database.JournalItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database
{
    public class Journal
    {
        public Guid Id { get; set; }
        public virtual List<JournalItem> JournalItems { get; set; }

        public Journal()
        {
            JournalItems = new List<JournalItem>();
        }
    }
}
