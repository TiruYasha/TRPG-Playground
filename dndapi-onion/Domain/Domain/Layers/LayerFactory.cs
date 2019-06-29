using System;
using System.Threading.Tasks;
using Domain.Dto.Shared;

namespace Domain.Domain.Layers
{
    public static class LayerFactory
    {
        public static Task<Layer> Create(LayerDto dto)
        {
            return Task.Run(() =>
            {
                switch (dto.Type)
                {
                    case LayerType.Default:
                        return new Layer(dto.Name);
                    case LayerType.Group:
                        return new Layer(dto.Name, LayerType.Group);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });
        }
    }
}
