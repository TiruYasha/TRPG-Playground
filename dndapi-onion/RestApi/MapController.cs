using Domain.Dto.Shared;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RestApi.Hubs;
using RestApi.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestApi
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "IsGamePlayer")]
    public class MapController : ControllerBase
    {

        private readonly IHubContext<GameHub> hubContext;
        private readonly IMapService mapService;
        private readonly IJwtReader jwtReader;

        public MapController(IMapService mapService, IJwtReader jwtReader, IHubContext<GameHub> hubContext)
        {
            this.mapService = mapService;
            this.jwtReader = jwtReader;
            this.hubContext = hubContext;
        }


        [HttpPut]
        [Authorize(Policy = "IsGameOwner")]
        public async Task<IActionResult> UpdateMap([FromBody] MapDto map)
        {
            var gameId = jwtReader.GetGameId();
            await mapService.UpdateMap(map, gameId);

            //TODO send event if the map is visible to players

            return Ok();
        }

        [HttpDelete]
        [Authorize(Policy = "IsGameOwner")]
        [Route("{mapId}")]
        public async Task<IActionResult> DeleteMap(Guid mapId)
        {
            var gameId = jwtReader.GetGameId();
            await mapService.DeleteMap(mapId, gameId);
            return Ok();
        }
    }
}
