using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Dto.RequestDto;
using Domain.Dto.Shared;

namespace Domain.ServiceInterfaces
{
    public interface IPlayAreaService
    {
        Task<MapDto> AddMap(AddMapDto dto, Guid playAreaId, Guid gameId);
    }
}
