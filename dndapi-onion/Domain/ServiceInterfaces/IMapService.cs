using Domain.Dto.RequestDto;
using Domain.Dto.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ServiceInterfaces
{
    public interface IMapService
    {
        Task UpdateMap(MapDto dto, Guid gameId);
        Task DeleteMap(Guid mapId, Guid gameId);

        Task<LayerDto> AddLayer(LayerDto dto, Guid mapId, Guid gameId);
        Task<LayerDto> UpdateLayer(LayerDto dto, Guid mapId, Guid gameId);
        Task DeleteLayer(Guid layerId, Guid mapId, Guid gameId);
        Task<IEnumerable<LayerDto>> GetLayers(Guid mapId, Guid gameId);
        Task UpdateLayerOrder(ChangeOrderDto dto, Guid layerId, Guid mapId, Guid gameId);
    }
}
