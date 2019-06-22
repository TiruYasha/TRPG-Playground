using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Dto.RequestDto;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApi.Utilities;

namespace RestApi
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "IsGameOwner")]
    public class PlayAreaController : ControllerBase
    {
        private readonly IPlayAreaService playAreaService;
        private readonly IJwtReader jwtReader;

        public PlayAreaController(IPlayAreaService playAreaService, IJwtReader jwtReader)
        {
            this.playAreaService = playAreaService;
            this.jwtReader = jwtReader;
        }

        [HttpPost]
        [Route("playarea/{playAreaId}/map")]
        public async Task<IActionResult> AddMapToPlayArea(Guid playAreaId, [FromBody] AddMapDto dto)
        {
            var gameId = jwtReader.GetGameId();

            var map = await playAreaService.AddMap(dto, playAreaId, gameId);

            return Ok(map);
        }
    }
}
