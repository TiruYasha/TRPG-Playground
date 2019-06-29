using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Dto.Shared;

namespace Domain.Domain.Layers
{
    public class LayerGroup: Layer
    {
        public virtual ICollection<Layer> Layers { get; private set; }
        public LayerGroup(string name) : base(name, LayerType.Group)
        {
            Layers = new List<Layer>();
        }

        public async Task<Layer> AddLayer(LayerDto dto)
        {
            var layer = await LayerFactory.Create(dto);

            Layers.Add(layer);
            return layer;
        }
    }
}
