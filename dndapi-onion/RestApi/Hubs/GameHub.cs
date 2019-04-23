using AutoMapper;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RestApi.Utilities;
using System;
using System.Threading.Tasks;

namespace RestApi.Hubs
{
    [Authorize]
    public class GameHub : Hub
    {
        private readonly IGameService gameService;
        private readonly IJwtReader jwtReader;
        private readonly IMapper mapper;

        public GameHub(IGameService gameService, IJwtReader jwtReader, IMapper mapper)
        {
            this.gameService = gameService;
            this.jwtReader = jwtReader;
            this.mapper = mapper;
        }

        public async Task AddToGroup(Guid gameId)
        {
            var userId = jwtReader.GetUserId();
            if (await gameService.IsGamePlayerOrOwnerOfGameAsync(userId, gameId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
            }
        }
    }
}
