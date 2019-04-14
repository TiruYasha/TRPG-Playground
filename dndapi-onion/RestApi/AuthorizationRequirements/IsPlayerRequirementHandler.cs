using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using RestApi.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestApi.Filters
{
    public class IsPlayerRequirementHandler : AuthorizationHandler<IsPlayerRequirement>
    {
        private readonly IJwtReader jwtReader;
        private readonly IGameService gameService;

        public IsPlayerRequirementHandler(IJwtReader jwtReader, IGameService gameService)
        {
            this.jwtReader = jwtReader;
            this.gameService = gameService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsPlayerRequirement requirement)
        {
            var activeGameId = jwtReader.GetGameId();
            var userId = jwtReader.GetUserId();

            bool isPlayer = await gameService.IsGamePlayerOrOwnerOfGameAsync(userId, activeGameId);

            if (isPlayer)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}
