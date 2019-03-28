using System;
using System.Collections.Generic;
using System.Text;

namespace Models.JournalItems
{
    public class HandoutModel : JournalItemModel
    {
        public HandoutModel() : base(2)
        {
        }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
