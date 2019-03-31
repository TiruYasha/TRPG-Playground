using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Domain
{
    public class Game
    {
        public Game()
        {
            Players = new List<GamePlayer>();
        }

        public Game(string name, User owner) : this()
        {
            CheckParameters(name, owner);
            Name = name;
            Owner = owner;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual User Owner { get; set; }
        public virtual ICollection<GamePlayer> Players { get; set; }

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
