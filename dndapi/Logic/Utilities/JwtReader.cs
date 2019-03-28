using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace Logic.Utilities
{
    public class JwtReader : IJwtReader
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtReader(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetUserId()
        {
            var id = _httpContextAccessor?.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return new Guid(id);
        }
    }
}
