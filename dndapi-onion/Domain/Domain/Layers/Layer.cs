using System;
using System.Threading.Tasks;
using Domain.Dto.Shared;

namespace Domain.Domain.Layers
{
    public class Layer
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }
        public LayerType Type { get; private set; }
        public int Order { get; set; }
        public Guid MapId { get; private set; }
        public virtual Map Map { get; private set; }

        protected Layer() { }

        public Layer(LayerDto dto, Guid mapId, LayerType type = LayerType.Default) : this()
        {
            CheckArguments(dto.Name);

            Id = Guid.NewGuid();
            Name = dto.Name;
            Order = dto.Order;
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

        public Task<Layer> UpdateOrder(int order)
        {
            return Task.Run(() =>
            {
                Order = order;
                return this;
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
