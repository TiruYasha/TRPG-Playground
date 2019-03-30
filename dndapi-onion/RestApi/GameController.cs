using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using RestApi.Models.Game;
using RestApi.Utilities;
using System;
using System.Threading.Tasks;

namespace RestApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService gameService;
        private readonly IJwtReader jwtReader;

        public GameController(IGameService gameService, IJwtReader jwtReader)
        {
            this.gameService = gameService;
            this.jwtReader = jwtReader;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateGameAsync([FromBody] CreateGameModel model)
        {
            try
            {
                var ownerId = jwtReader.GetUserId();

                await gameService.CreateGameAsync(model.Name, ownerId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
