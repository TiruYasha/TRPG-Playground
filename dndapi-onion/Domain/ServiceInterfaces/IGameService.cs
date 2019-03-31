using Domain.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.ServiceInterfaces
{
    public interface IGameService
    {
        Task CreateGameAsync(string gameName, Guid ownerId);
        Task JoinGameAsync(Guid gameId, Guid userId);
        IList<Game> GetAllGames();
    }
}
