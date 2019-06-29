﻿using Domain.Dto.Shared;
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

        [HttpPost]
        [Authorize(Policy = "IsGameOwner")]
        [Route("{mapId}/layer")]
        public async Task<IActionResult> AddLayer(Guid mapId, [FromBody] LayerDto dto)
        {
            var gameId = jwtReader.GetGameId();
            var result = await mapService.AddLayer(dto, mapId, gameId);

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = "IsGameOwner")]
        [Route("{mapId}/layer")]
        public async Task<IActionResult> GetLayers(Guid mapId)
        {
            var gameId = jwtReader.GetGameId();
            var result = await mapService.GetLayers(mapId, gameId);

            return Ok(result);
        }

        [HttpPut]
        [Authorize(Policy = "IsGameOwner")]
        [Route("{mapId}/layer")]
        public async Task<IActionResult> UpdateLayer(Guid mapId, [FromBody] LayerDto dto)
        {
            var gameId = jwtReader.GetGameId();
            var result = await mapService.UpdateLayer(dto, mapId, gameId);

            return Ok(result);
        }

        [HttpDelete]
        [Authorize(Policy = "IsGameOwner")]
        [Route("{mapId}/layer/{layerId}")]
        public async Task<IActionResult> GetLayers(Guid mapId, Guid layerId)
        {
            var gameId = jwtReader.GetGameId();
            await mapService.DeleteLayer(layerId, mapId, gameId);

            return Ok();
        }
    }
}
