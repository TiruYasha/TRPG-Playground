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
        public virtual Game Game { get; set; }
        public Guid GameId { get; }
        public virtual JournalFolder ParentFolder { get; set; }
        public Guid? ParentFolderId { get; set; }
        public virtual ICollection<JournalItemPemission> Permissions { get; set; }

        public JournalItem() { }

        public JournalItem(JournalItemType type, string name, Guid gameId, string imageId, ICollection<Guid> canSee, ICollection<Guid> canEdit)
        {
            Permissions = new List<JournalItemPemission>();
            CheckArguments(name);
            Id = Guid.NewGuid();
            Type = type;
            Name = name;
            GameId = gameId;
            ImageId = imageId;
            CreatedOn = DateTime.UtcNow;
            LastEditedOn = DateTime.UtcNow;

            SetPermissions(canSee, canEdit);
        }

        private void SetPermissions(ICollection<Guid> canSee, ICollection<Guid> canEdit)
        {
            if(canSee == null) { return; }
            foreach (var s in canSee)
            {
                if (canEdit.Contains(s))
                {
                    Permissions.Add(new JournalItemPemission(Id, s, GameId, true, true));
                }
                else
                {
                    Permissions.Add(new JournalItemPemission(Id, s, GameId, true));
                }
            }
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
