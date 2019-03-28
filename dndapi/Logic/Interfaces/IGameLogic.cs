using Database;
using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public interface IGameLogic
    {
        Task<GameModel> CreateGameAsync(GameModel model);
        Task JoinGameAsync(GameModel model);
        List<GameModel> GetAllGames();
    }
}
