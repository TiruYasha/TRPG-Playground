using System;

namespace Domain.Domain
{
    public class Game
    {
        public Game()
        {

        }
        public Game(string name, User owner)
        {
            CheckParameters(name, owner);
            Name = name;
            Owner = owner;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual User Owner { get; set; }

        private void CheckParameters(string name, User owner)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("The argument for parameter name was invalid");
            }

            if(owner == null)
            {
                throw new ArgumentException("The argument for parameter owner was invalid");
            }
        }
    }
}
