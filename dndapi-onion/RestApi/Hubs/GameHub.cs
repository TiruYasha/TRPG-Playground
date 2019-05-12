using AutoMapper;
using Domain.Domain;
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
        private readonly IAccountService accountService;
        private readonly IJwtReader jwtReader;
        private readonly IMapper mapper;

        public GameHub(IGameService gameService, IAccountService accountService, IJwtReader jwtReader, IMapper mapper)
        {
            this.gameService = gameService;
            this.accountService = accountService;
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

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = jwtReader.GetUserId();
        }
    }
}
