using Database;
using Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repository;
using System.Threading.Tasks;

namespace DndApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class GameController : Controller
    {
        private readonly IGameLogic _gameLogic;

        public GameController(IGameLogic gameLogic)
        {
            _gameLogic = gameLogic;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGame([FromBody] GameModel gameModel)
        {
            var game = await _gameLogic.CreateGameAsync(gameModel);

            return Ok(game);
        }

        [HttpPut]
        public async Task<IActionResult> JoinGame([FromBody] GameModel gameModel)
        {
            await _gameLogic.JoinGameAsync(gameModel);

            return Ok();
        }

        [HttpGet]
        public IActionResult GetGames()
        {
            var games = _gameLogic.GetAllGames();

            return Ok(games);
        }
    }
}
