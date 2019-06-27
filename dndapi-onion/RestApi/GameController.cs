using AutoMapper;
using Domain.Domain;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApi.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Dto.RequestDto;
using Domain.Dto.RequestDto.Game;
using Domain.Dto.Shared;

namespace RestApi
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        public async Task<IActionResult> GetAllGames()
        {
            var games = await gameService.GetAllGames();

            return Ok(games);
        }

        [HttpPut]
        [Route("join")]
        public async Task<IActionResult> JoinGameAsync()
        {
            try
            {
                var userId = jwtReader.GetUserId();
                var gameId = jwtReader.GetGameId();

                var result = await gameService.JoinGameAsync(gameId, userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("players")]
        [Authorize(Policy = "IsGamePlayer")]
        public async Task<IActionResult> GetAllPlayersAsync()
        {
            var gameId = jwtReader.GetGameId();

            var result = await gameService.GetPlayersAsync(gameId);

            return Ok(result);
        }

        [HttpPost]
        [Route("map")]
        public async Task<IActionResult> AddMapToPlayArea([FromBody] MapDto dto)
        {
            var gameId = jwtReader.GetGameId();

            var map = await gameService.AddMap(dto, gameId);

            return Ok(map);
        }

        [HttpGet]
        [Route("map")]
        public async Task<IActionResult> GetMaps()
        {
            var gameId = jwtReader.GetGameId();

            var maps = await gameService.GetMaps(gameId);

            return Ok(maps);
        }
    }
}
