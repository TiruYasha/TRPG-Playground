using System;

namespace Domain.Domain
{
    public class Game
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual User Owner { get; set; }
    }
}
