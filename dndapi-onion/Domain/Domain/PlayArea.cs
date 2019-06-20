using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Dto.RequestDto;

namespace Domain.Domain
{
    public class PlayArea
    {
        public Guid Id { get; private set; }
        public Guid GameId { get; private set; }
        public virtual Game Game { get; private set; }
        public virtual ICollection<Map> Maps { get; private set; }

        private PlayArea() { }

        public PlayArea(Guid gameId) : this()
        {
            Id = Guid.NewGuid();
            Maps = new List<Map>();
            GameId = gameId;
        }

        public Task<Map> AddMap(AddMapDto dto)
        {
            return Task.Run(() =>
             {
                 var map = new Map(dto);
                 Maps.Add(map);
                 return map;
             });
        }
    }
}