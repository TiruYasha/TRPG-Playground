using System;
using System.Threading.Tasks;
using Domain.Dto.Shared;

namespace Domain.Domain.Layers
{
    public static class LayerFactory
    {
        public static Task<Layer> Create(LayerDto dto, Guid mapId)
        {
            return Task.Run(() =>
            {
                switch (dto.Type)
                {
                    case LayerType.Default:
                        return new Layer(dto.Name, mapId);
                    case LayerType.Group:
                        return new LayerGroup(dto.Name, mapId);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });
        }
    }
}
