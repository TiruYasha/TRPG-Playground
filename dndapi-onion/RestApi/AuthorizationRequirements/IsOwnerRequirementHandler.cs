using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using RestApi.Utilities;
using System.Threading.Tasks;

namespace RestApi.Filters
{
    public class IsOwnerRequirementHandler : AuthorizationHandler<IsOwnerRequirement>
    {
        private readonly IJwtReader jwtReader;
        private readonly IGameService gameService;
        public IsOwnerRequirementHandler(IJwtReader jwtReader, IGameService gameService)
        {
            this.jwtReader = jwtReader;
            this.gameService = gameService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsOwnerRequirement requirement)
        {
            var activeGameId = jwtReader.GetGameId();
            var userId = jwtReader.GetUserId();

            bool isOwner = await gameService.IsOwnerOfGameAsync(userId, activeGameId);

            if (isOwner)
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
