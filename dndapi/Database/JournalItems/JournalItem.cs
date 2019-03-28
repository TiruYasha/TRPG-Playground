using System;

namespace Database.JournalItems
{
    public class JournalItem
    {
        public Guid Id { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }

        public JournalItem(int type)
        {
            Type = type;
        }
    }
}