using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Dto.Shared;

namespace Domain.Domain.Layers
{
    public class LayerGroup : Layer
    {
        public virtual ICollection<Layer> Layers { get; private set; }

        private LayerGroup()
        {
            Layers = new List<Layer>();
        }
        public LayerGroup(string name, Guid mapId) : base(name, mapId, LayerType.Group)
        {
            Layers = new List<Layer>();
        }

        public async Task<Layer> AddLayer(LayerDto dto, Guid mapId)
        {
            var layer = await LayerFactory.Create(dto, mapId);

            Layers.Add(layer);
            return layer;
        }
    }
}
