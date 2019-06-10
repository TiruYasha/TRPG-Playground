using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Dto.Shared;

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

        protected JournalItem(JournalItemDto dto, Guid gameId)
        {
           
            CheckArguments(dto.Name);
            Id = Guid.NewGuid();
            Type = dto.Type;
            Name = dto.Name;
            GameId = gameId;
            CreatedOn = DateTime.UtcNow;
            LastEditedOn = DateTime.UtcNow;

            SetPermissions(dto.Permissions);
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

        public virtual Task Update(JournalItemDto dto)
        {
            return Task.Run(() =>
            {
                CheckArguments(dto.Name);
                Name = dto.Name;
                LastEditedOn = DateTime.UtcNow;

                SetPermissions(dto.Permissions);
            });
        }

        private void SetPermissions(IEnumerable<JournalItemPermissionDto> permissions)
        {
            Permissions = new List<JournalItemPermission>();
            foreach (var permission in permissions)
            {
                var newPermission = new JournalItemPermission(Id, permission.UserId, GameId, permission.CanSee, permission.CanEdit);
                Permissions.Add(newPermission);
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
