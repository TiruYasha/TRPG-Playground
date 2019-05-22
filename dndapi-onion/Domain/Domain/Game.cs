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
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual User Owner { get; set; }
        public virtual Guid OwnerId { get; set; }
        public virtual ICollection<GamePlayer> Players { get; set; }
        public virtual ICollection<ChatMessage> ChatMessages { get; set; }
        public virtual ICollection<JournalItem> JournalItems { get; set; }

        public Game()
        {
            Players = new List<GamePlayer>();
            ChatMessages = new List<ChatMessage>();
            JournalItems = new List<JournalItem>();
        }

        public Game(string name, User owner) : this()
        {
            CheckParameters(name, owner);
            Name = name;
            Owner = owner;
            Id = Guid.NewGuid();
        }

        public Game(string name, Guid ownerId) : this()
        {
            Name = name;
            OwnerId = ownerId;
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

        public virtual Task<JournalItem> AddJournalItemAsync(AddJournalItemDto dto)
        {
            return Task.Run(() =>
            {
                var journalItem = JournalItemFactory.Create(dto, Id);

                JournalItems.Add(journalItem);

                return journalItem;
            });
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
