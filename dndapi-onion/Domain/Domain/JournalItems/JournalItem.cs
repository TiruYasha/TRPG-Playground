using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Domain.JournalItems
{
    public abstract class JournalItem
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public JournalItemType Type { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime LastEditedOn { get; private set; }
        public Guid? ImageId { get; private set; }
        public virtual Image Image { get; private set; }
        public virtual Game Game { get; private set; }
        public Guid GameId { get; private set; }
        public virtual JournalFolder ParentFolder { get; private set; }
        public Guid? ParentFolderId { get; private set; }
        public virtual ICollection<JournalItemPermission> Permissions { get; private set; }

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
