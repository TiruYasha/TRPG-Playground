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
    }
}
