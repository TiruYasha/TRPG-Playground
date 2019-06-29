using System;
using System.Threading.Tasks;

namespace Domain.Domain.Layers
{
    public class Layer
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }
        public LayerType Type { get; private set; }

        public Guid MapId { get; private set; }
        public virtual Map Map { get; private set; }
        public virtual LayerGroup LayerGroup { get; private set; }
        public virtual Guid? LayerGroupId { get; private set; }

        private Layer() { }

        public Layer(string name, LayerType type = LayerType.Default) : this()
        {
            CheckArguments(name);

            Id = Guid.NewGuid();
            Name = name;
            Type = type;
        }

        public Task Update(string name)
        {
            return Task.Run(() =>
            {
                CheckArguments(name);
                Name = name;
            });
        }

        private static void CheckArguments(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name may not be empty");
            }
        }
    }
}
