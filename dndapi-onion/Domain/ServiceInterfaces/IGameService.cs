using Domain.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        IList<Game> GetAllGames();
        Task<bool> IsOwnerOfGameAsync(Guid userId, Guid activeGameId);
        Task<bool> IsGamePlayerOrOwnerOfGameAsync(Guid userId, Guid activeGameId);

    }
}
