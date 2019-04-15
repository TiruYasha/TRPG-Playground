using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestApi.Hubs
{
    [Authorize]
    public class GameHub : Hub
    {
    }
}
