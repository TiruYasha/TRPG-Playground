using System;

namespace Domain.Dto.ReturnDto.Game
{
    public class GetPlayersModel
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
    }
}
