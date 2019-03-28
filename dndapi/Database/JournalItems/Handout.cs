using System;
using System.Collections.Generic;
using System.Text;

namespace Database.JournalItems
{
    public class Handout : JournalItem
    {
        public string Description { get; set; }
        public Handout() : base(2)
        {
        }
    }
}
