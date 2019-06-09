using Domain.Domain.JournalItems;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Dto.RequestDto.Journal;
using Domain.Dto.Shared;

namespace Domain.Domain
{
    public class Game
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public virtual User Owner { get; private set; }
        public Guid OwnerId { get; private set; }
        public virtual ICollection<GamePlayer> Players { get; private set; }
        public virtual ICollection<ChatMessage> ChatMessages { get; private set; }
        public virtual ICollection<JournalItem> JournalItems { get; private set; }

        private Game()
        {
            Id = Guid.NewGuid();
            Players = new List<GamePlayer>();
            ChatMessages = new List<ChatMessage>();
            JournalItems = new List<JournalItem>();
        }

        public Game(string name, User owner) : this()
        {
            CheckParameters(name, owner);
            Name = name;
            Owner = owner;
        }

        public Game(string name, Guid ownerId) : this()
        {
            Name = name;
            OwnerId = ownerId;
            
        }

        public async Task Join(User user)
        {

            if (await HasPlayerJoined(user.Id) || await IsOwner(user.Id))
            {
                throw new ArgumentException("The player cannot be added again");
            }

            var newPlayer = new GamePlayer(this, user);
            Players.Add(newPlayer);
        }

        public Task<bool> HasPlayerJoined(Guid userId)
        {
            return Task.Run(() => Players.Any(u => u.UserId == userId));
        }

        public Task<bool> IsOwner(Guid ownerId)
        {
            return Task.Run(() => Owner.Id == ownerId);
        }

        public Task<ChatMessage> AddChatMessageAsync(string message, string customUsername, Guid userId)
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

        public Task<JournalItem> AddJournalItem(JournalItemDto dto)
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
