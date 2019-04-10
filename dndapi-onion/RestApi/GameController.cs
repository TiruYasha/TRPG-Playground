using AutoMapper;
using Domain.Domain;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApi.Models.Game;
using RestApi.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestApi
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GameController : ControllerBase
    {
        private readonly IGameService gameService;
        private readonly IJwtReader jwtReader;
        private readonly IMapper mapper;

        public GameController(IGameService gameService, IJwtReader jwtReader, IMapper mapper)
        {
            this.gameService = gameService;
            this.jwtReader = jwtReader;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateGameAsync([FromBody] CreateGameModel model)
        {
            try
            {
                var ownerId = jwtReader.GetUserId();

                var gameId = await gameService.CreateGameAsync(model.Name, ownerId);

                return Ok(gameId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("all")]
        public IActionResult GetAllGames()
        {
            var games = gameService.GetAllGames();
            var mappedGames = mapper.Map<IList<Game>, IList<GameCatalogItemModel>>(games);

            return Ok(mappedGames);
        }

        [HttpPut]
        [Route("join")]
        public async Task<IActionResult> JoinGameAsync([FromBody] JoinGameModel model)
        {
            try
            {
                var userId = jwtReader.GetUserId();

                var result = await gameService.JoinGameAsync(model.GameId, userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
