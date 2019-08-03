using Domain.Domain;
using Domain.Dto.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Dto.ReturnDto.Game
{
    public class InitialGameDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsOwner { get; set; }
        public MapDto VisibleMap { get; set; }
        public IEnumerable<GetPlayersModel> Players { get; set; }

        public InitialGameDto()
        {
            Players = new List<GetPlayersModel>();
        }
    }
}
