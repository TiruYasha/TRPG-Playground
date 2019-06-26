﻿using AutoMapper;
using DataAccess;
using Domain.Dto.RequestDto;
using Domain.Dto.Shared;
using Domain.Exceptions;
using Domain.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class MapService : IMapService
    {
        private DndContext context;
        private IMapper mapper;

        public MapService(DndContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        
        public async Task DeleteMap(Guid mapId, Guid gameId)
        {
            var map = await context.Games.FilterById(gameId).SelectMany(g => g.Maps.Where(m => m.Id == mapId)).FirstOrDefaultAsync();
            context.Maps.Remove(map);
            await context.SaveChangesAsync();
        }

        public async Task UpdateMap(MapDto dto, Guid gameId)
        {
            var map = await context.Games.FilterById(gameId).SelectMany(g => g.Maps.Where(m => m.Id == dto.Id)).FirstOrDefaultAsync();

            if (map == null) { throw new NotFoundException("The map could not be found"); }

            await map.Update(dto);

            await context.SaveChangesAsync();
        }
    }
}
