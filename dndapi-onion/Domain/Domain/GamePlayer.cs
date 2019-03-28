using System;

namespace Domain.Domain
{
    public class GamePlayer
    {
        public Guid Id { get; set; }
        public virtual User User { get; set; }
        public virtual Game Game { get; set; }
    }
}
