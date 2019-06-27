using System;

namespace Domain.Domain
{
    public class Layer
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public Guid MapId { get; private set; }
        public virtual Map Map { get; private set; }

        private Layer() { }

        public Layer(string name) : this()
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name may not be empty");
            }

            Id = Guid.NewGuid();
            Name = name;
        }
    }
}
