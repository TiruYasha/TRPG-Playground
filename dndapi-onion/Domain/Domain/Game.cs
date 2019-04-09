using Domain.Domain.JournalItems;
using Domain.Exceptions;
using Domain.RequestModels.Journal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Domain
{
    public class Game
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public virtual User Owner { get; private set; }
        public virtual ICollection<GamePlayer> Players { get; private set; }
        public virtual ICollection<ChatMessage> ChatMessages { get; private set; }
        public virtual ICollection<JournalItem> JournalItems { get; private set; }

        public Game()
        {
            Players = new List<GamePlayer>();
            ChatMessages = new List<ChatMessage>();
            JournalItems = new List<JournalItem>();
        }

        public Game(string name, Guid id)
        {
            Name = name;
            Id = id;
        }

        public Game(string name, User owner) : this()
        {
            CheckParameters(name, owner);
            Name = name;
            Owner = owner;
            Id = Guid.NewGuid();
        }

        public void Join(User user)
        {
            if (HasPlayerJoined(user.Id) || IsOwner(user.Id))
            {
                throw new ArgumentException("The player cannot be added again");
            }

            var newPlayer = new GamePlayer(this, user);
            Players.Add(newPlayer);
        }

        public bool HasPlayerJoined(Guid userId)
        {
            return Players.Any(u => u.UserId == userId);
        }

        public bool IsOwner(Guid ownerId)
        {
            return Owner.Id == ownerId;
        }

        public virtual Task<ChatMessage> AddChatMessageAsync(string message, string customUsername, Guid userId)
        {
            return Task.Run(() =>
            {
                var player = Players.FirstOrDefault(p => p.UserId == userId);
                ChatMessage chatMessage = null;
                if (Owner.Id == userId)
                {
                    chatMessage = new ChatMessage(message, customUsername, Owner, this);
                }
                else if (player != null)
                {
                    chatMessage = new ChatMessage(message, customUsername, player.User, this);
                }
                else
                {
                    throw new PlayerDoesNotExistException("The player does not exist in this game");
                }

                ChatMessages.Add(chatMessage);
                return chatMessage;
            });
        }

        public virtual Task<JournalFolder> AddJournalFolderAsync(AddFolderModel model, Guid userId)
        {
            return Task.Run(() =>
            {
                if (Owner.Id != userId)
                {
                    throw new PermissionException("This is an illegal action! It is only possible for gamemasters to add journal items.");
                }

                var folder = new JournalFolder(model.Name);
                var parent = GetParentFolder(model);

                if (parent != null)
                {
                    parent.AddJournalItem(folder);
                }
                else
                {
                    JournalItems.Add(folder);
                }

                return folder;
            });
        }

        private JournalFolder GetParentFolder(AddFolderModel model)
        {
            var items = JournalItems.Select(s => s as JournalFolder).ToList();
            return GetParentsFolderRecursion(items, model.ParentFolderId);
        }

        private JournalFolder GetParentsFolderRecursion(ICollection<JournalFolder> folders, Guid parentId)
        {
            if (folders == null)
            {
                return null;
            }

            var folder = folders.FirstOrDefault(f => f.Id == parentId);

            if (folder != null)
                return folder;

            foreach (var item in folders)
            {
                var items = GetJournalFoldersFromJournalItems(item.JournalItems);

                return GetParentsFolderRecursion(items, parentId);
            }

            return null;
        }

        private List<JournalFolder> GetJournalFoldersFromJournalItems(ICollection<JournalItem> journalItems)
        {
            return journalItems.Where(j => j.GetType() == typeof(JournalFolder)).Select(s => s as JournalFolder).ToList();
        }

        private void CheckParameters(string name, User owner)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("The argument for parameter name was invalid");
            }

            if (owner == null)
            {
                throw new ArgumentException("The argument for parameter owner was invalid");
            }
        }
    }
}
