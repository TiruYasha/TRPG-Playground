﻿using System;

namespace Domain.Domain.JournalItems
{
    public abstract class JournalItem
    {
        public Guid Id { get; set; }
        public string Name { get; set;  }
        public JournalItemType Type { get; set; }

        private JournalItem() { }

        public JournalItem(JournalItemType type, string name)
        {
            Id = Guid.NewGuid();
            this.Type = type;
            this.Name = name;
        }
    }
}