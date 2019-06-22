using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess;
using Domain.Domain;
using Domain.Dto.RequestDto;
using Domain.Dto.Shared;
using Domain.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class PlayAreaService : IPlayAreaService
    {
        private readonly DndContext context;
        private readonly IMapper mapper;

        public PlayAreaService(DndContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<MapDto> AddMap(AddMapDto dto, Guid playAreaId, Guid gameId)
        {
            var playArea = await context.PlayAreas.FilterByGameAndPlayAreaId(gameId, playAreaId).FirstOrDefaultAsync();

            var map = await playArea.AddMap(dto);

            await context.SaveChangesAsync();

            return mapper.Map<Map, MapDto>(map);
        }
    }
}
