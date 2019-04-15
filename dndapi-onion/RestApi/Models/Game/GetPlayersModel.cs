using System;
using System.Collections.Generic;
using System.Text;

namespace RestApi.Models.Game
{
    public class GetPlayersModel
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
    }
}
