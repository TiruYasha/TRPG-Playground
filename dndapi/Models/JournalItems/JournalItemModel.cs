using System;
using System.Collections.Generic;
using System.Text;

namespace Models.JournalItems
{
    public class JournalItemModel
    {
        public Guid Id { get; set; }
        public int Type { get; set; }

        public JournalItemModel(int type)
        {
            Type = type;
        }

    }
}
