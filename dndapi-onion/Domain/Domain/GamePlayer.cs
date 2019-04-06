using System;

namespace Domain.Domain
{
    public class GamePlayer
    {
        public Guid UserId { get; private set; }
        public virtual User User { get; private set; }
        public Guid GameId { get; private set; }
        public virtual Game Game { get; private set; }

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
