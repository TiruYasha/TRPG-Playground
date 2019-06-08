using Microsoft.AspNetCore.Authorization;

namespace RestApi.AuthorizationRequirements
{
    public class IsOwnerRequirement : IAuthorizationRequirement
    {
        public IsOwnerRequirement() {
            
        }
    }
}
