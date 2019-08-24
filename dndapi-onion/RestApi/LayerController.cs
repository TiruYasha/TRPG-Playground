using Domain.Dto.Shared;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RestApi.AuthorizationRequirements;
using RestApi.Events;
using RestApi.Hubs;
using RestApi.Utilities;
using System;
using System.Threading.Tasks;

namespace RestApi
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = AuthorizationRequirement.IsGameOwner)]
    public class LayerController : ControllerBase
    {
        private readonly IHubContext<GameHub> hubContext;
        private readonly ILayerService layerService;
        private readonly IJwtReader jwtReader;

        public LayerController(ILayerService layerService, IJwtReader jwtReader, IHubContext<GameHub> hubContext)
        {
            this.layerService = layerService;
            this.jwtReader = jwtReader;
            this.hubContext = hubContext;
        }

        [HttpPost]
        [Route("{layerId}/token")]
        public async Task<IActionResult> AddToken([FromBody] TokenDto tokenDto, Guid layerId)
        {
            var gameId = jwtReader.GetGameId();
            var userId = jwtReader.GetUserId();

            var token = await layerService.AddTokenToLayer(tokenDto, gameId, userId, layerId);

            return Ok(token);
        }

        [HttpPost]
        [Route("{layerId}/visibleForPlayers")]
        public async Task<IActionResult> TogglePlayerVisibility(Guid layerId)
        {
            var gameId = jwtReader.GetGameId();

            await layerService.ToggleVisibleForPlayers(gameId, layerId);

            await hubContext.Clients.Group(gameId.ToString()).SendAsync(LayerEvents.LayerVisibilityChanged, layerId);

            return Ok();
        }
    }
}
