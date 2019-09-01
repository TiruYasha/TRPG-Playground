using Domain.Dto.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ServiceInterfaces
{
    public interface ILayerService
    {
        Task<TokenDto> AddTokenToLayer(TokenDto dto, Guid gameId, Guid userId, Guid layerId);
        Task DeleteToken(Guid tokenId, Guid gameId, Guid layerId);

        Task ToggleVisibleForPlayers(Guid gameId, Guid layerId);
        Task ToggleVisible(Guid gameId, Guid layerId);
    }
}
