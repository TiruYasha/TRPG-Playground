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
        public Guid? LayerGroupId { get; private set; }

        protected Layer() { }

        public Layer(string name, Guid mapId, LayerType type = LayerType.Default) : this()
        {
            CheckArguments(name);

            Id = Guid.NewGuid();
            Name = name;
            Type = type;
            MapId = mapId;
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
