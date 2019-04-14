using System;

namespace RestApi.Utilities
{
    public interface IJwtReader
    {
        Guid GetUserId();
        Guid GetGameId();
    }
}
