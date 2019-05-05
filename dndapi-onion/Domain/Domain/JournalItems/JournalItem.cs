using Domain.Exceptions;
using System;
using System.Collections.Generic;

namespace Domain.Domain.JournalItems
{
    public abstract class JournalItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageId { get; set; }
        public JournalItemType Type { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastEditedOn { get; set; }
        public virtual ICollection<JournalItemPemission> Pemissions { get; set; }

        private JournalItem() { }

        public JournalItem(JournalItemType type, string name, string imagePath, ICollection<User> canSee, ICollection<User> canEdit)
        {
            CheckArguments(name);
            Id = Guid.NewGuid();
            Type = type;
            Name = name;
            ImageId = imagePath;
            CreatedOn = DateTime.UtcNow;
            LastEditedOn = DateTime.UtcNow;
        }

        private void CheckArguments(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new JournalItemException("The name is empty");
            }
        }
    }
}
