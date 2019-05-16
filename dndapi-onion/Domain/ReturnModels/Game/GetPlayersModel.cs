using System;

namespace Domain.ReturnModels.Game
{
    public class GetPlayersModel
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
    }
}
