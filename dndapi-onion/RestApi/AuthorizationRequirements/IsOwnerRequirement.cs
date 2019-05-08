using Microsoft.AspNetCore.Authorization;

namespace RestApi.Filters
{
    public class IsOwnerRequirement : IAuthorizationRequirement
    {
        public IsOwnerRequirement() {
            
        }
    }
}
