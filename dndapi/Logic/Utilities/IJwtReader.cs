using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Utilities
{
    public interface IJwtReader
    {
        Guid GetUserId();
    }
}
