using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace RestApi.Utilities
{
    public class JwtReader : IJwtReader
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public JwtReader(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public Guid GetUserId()
        {
            var id = httpContextAccessor?.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return new Guid(id);
        }
    }
}
