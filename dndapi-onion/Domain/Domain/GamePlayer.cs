using Domain.Domain.JournalItems;
using System;
using System.Collections.Generic;

namespace Domain.Domain
{
    public class GamePlayer
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public Guid GameId { get; set; }
        public virtual Game Game { get; set; }
        public virtual ICollection<JournalItemPermission> JournalItemPermissions { get; set; }
        public GamePlayer() { }

        public GamePlayer(Game game, User user)
        {
            CheckValues(game, user);

            UserId = user.Id;
            GameId = game.Id;
        }

        private void CheckValues(Game game, User user)
        {
            if(user == null)
            {
                throw new ArgumentNullException("user");
            }

            if(game == null)
            {
                throw new ArgumentNullException("game");
            }
        }
    }
}
