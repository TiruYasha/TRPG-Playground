using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Domain.JournalItems
{
    public abstract class JournalItem
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }

        public virtual JournalItemType Type { get; set; }
        public virtual DateTime CreatedOn { get; set; }
        public virtual DateTime LastEditedOn { get; set; }
        public virtual Guid? ImageId { get; private set; }
        public virtual Image Image { get; set; }
        public virtual Game Game { get; set; }
        public virtual Guid GameId { get; set; }
        public virtual JournalFolder ParentFolder { get; set; }
        public virtual Guid? ParentFolderId { get; set; }
        public virtual ICollection<JournalItemPermission> Permissions { get; set; }

        protected JournalItem() { }

        protected JournalItem(JournalItemType type, string name, Guid gameId, ICollection<Guid> canSee, ICollection<Guid> canEdit)
        {
            Permissions = new List<JournalItemPermission>();
            CheckArguments(name);
            Id = Guid.NewGuid();
            Type = type;
            Name = name;
            GameId = gameId;
            CreatedOn = DateTime.UtcNow;
            LastEditedOn = DateTime.UtcNow;

            SetPermissions(canSee, canEdit);
        }

        public Task<Image> SetImage(string extension, string originalName)
        {
            return Task.Run(() =>
            {
                var image = new Image(extension, originalName);

                Image = image;

                return image;
            });
        }

        private void SetPermissions(ICollection<Guid> canSee, ICollection<Guid> canEdit)
        {
            if (canSee == null) { return; }
            foreach (var s in canSee)
            {
                Permissions.Add(canEdit.Contains(s)
                    ? new JournalItemPermission(Id, s, GameId, true, true)
                    : new JournalItemPermission(Id, s, GameId, true));
            }
        }

        private static void CheckArguments(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new JournalItemException("The name is empty");
            }
        }
    }
}
