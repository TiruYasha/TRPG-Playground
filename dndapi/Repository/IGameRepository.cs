using Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IGameRepository
    {
        Task InsertGameAsync(Game game);
        List<Game> GetAllGames();
        Task<Game> FindGameByIdAsync(Guid gameId);
        Task UpdateGameAsync(Game game);
        Task AddMessageAsync(ChatMessage message);
    }
}
