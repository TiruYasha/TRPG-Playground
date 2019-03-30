using System;
using System.Collections.Generic;
using System.Text;

namespace RestApi.Utilities
{
    public interface IJwtReader
    {
        Guid GetUserId();
    }
}
