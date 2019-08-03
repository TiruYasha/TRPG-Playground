using Domain.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Dto.RequestDto;
using Domain.Dto.ReturnDto.Game;
using Domain.Dto.Shared;

namespace Domain.ServiceInterfaces
{
    public interface IGameService
    {
        Task<Guid> CreateGameAsync(string gameName, Guid ownerId);
        /// <summary>
        /// This adds the player to the game
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="userId"></param>
        /// <returns>Returns a boolean. When the boolean is true it is the owner of the game. If it is false it is a player</returns>
        Task<bool> JoinGameAsync(Guid gameId, Guid userId);
        Task<IEnumerable<GameCatalogItemModel>> GetAllGames();
        Task<bool> IsOwnerOfGameAsync(Guid userId, Guid activeGameId);
        Task<bool> IsGamePlayerOrOwnerOfGameAsync(Guid userId, Guid activeGameId);
        Task<IEnumerable<GetPlayersModel>> GetPlayersAsync(Guid gameId);
        Task<MapDto> AddMap(MapDto dto, Guid gameId);
        Task<IEnumerable<MapDto>> GetMaps(Guid gameId);
        Task<MapDto> SetMapVisible(Guid gameId, Guid mapId);
    }
}
