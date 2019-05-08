using Domain.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface IGameRepository
    {
        Task CreateGameAsync(Game game);
        Task UpdateGameAsync(Game game);
        Task<Game> GetGameByIdAsync(Guid gameId);
        IQueryable<Game> GetAllGames();
        Task<bool> IsOwnerOfGameAsync(Guid userId, Guid activeGameId);
        Task<bool> IsGamePlayerOrOwnerOfGameAsync(Guid userId, Guid activeGameId);
        Task<IQueryable<GamePlayer>> GetPlayersFromGameAsync(Guid gameId);
    }
}
