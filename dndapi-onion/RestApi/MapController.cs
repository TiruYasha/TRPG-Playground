using Domain.Dto.Shared;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RestApi.AuthorizationRequirements;
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
    [Authorize(Policy = AuthorizationRequirement.IsGamePlayer)]
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
        [Authorize(Policy = AuthorizationRequirement.IsGameOwner)]
        public async Task<IActionResult> UpdateMap([FromBody] MapDto map)
        {
            var gameId = jwtReader.GetGameId();
            await mapService.UpdateMap(map, gameId);

            return Ok();
        }

        [HttpDelete]
        [Authorize(Policy = AuthorizationRequirement.IsGameOwner)]
        [Route("{mapId}")]
        public async Task<IActionResult> DeleteMap(Guid mapId)
        {
            var gameId = jwtReader.GetGameId();
            await mapService.DeleteMap(mapId, gameId);
            return Ok();
        }

        [HttpPost]
        [Authorize(Policy = AuthorizationRequirement.IsGameOwner)]
        [Route("{mapId}/layer")]
        public async Task<IActionResult> AddLayer(Guid mapId, [FromBody] LayerDto dto)
        {
            var gameId = jwtReader.GetGameId();
            var result = await mapService.AddLayer(dto, mapId, gameId);

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = AuthorizationRequirement.IsGameOwner)]
        [Route("{mapId}/layer")]
        public async Task<IActionResult> GetLayers(Guid mapId)
        {
            var gameId = jwtReader.GetGameId();
            var result = await mapService.GetLayers(mapId, gameId);

            return Ok(result);
        }

        [HttpPut]
        [Authorize(Policy = AuthorizationRequirement.IsGameOwner)]
        [Route("{mapId}/layer")]
        public async Task<IActionResult> UpdateLayer(Guid mapId, [FromBody] LayerDto dto)
        {
            var gameId = jwtReader.GetGameId();
            var result = await mapService.UpdateLayer(dto, mapId, gameId);

            return Ok(result);
        }

        [HttpDelete]
        [Authorize(Policy = AuthorizationRequirement.IsGameOwner)]
        [Route("{mapId}/layer/{layerId}")]
        public async Task<IActionResult> DeleteLayer(Guid mapId, Guid layerId)
        {
            var gameId = jwtReader.GetGameId();
            await mapService.DeleteLayer(layerId, mapId, gameId);

            return Ok();
        }

        [HttpPut]
        [Authorize(Policy = AuthorizationRequirement.IsGameOwner)]
        [Route("{mapId}/layer/{layerId}/order")]
        public async Task<IActionResult> UpdateOrder([FromBody] ChangeOrderDto dto, Guid mapId, Guid layerId)
        {
            var gameId = jwtReader.GetGameId();
            await mapService.UpdateLayerOrder(dto, layerId, mapId, gameId);

            return Ok();
        }
    }
}
