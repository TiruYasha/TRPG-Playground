using AutoMapper;
using DataAccess;
using Domain.Dto.Shared;
using Domain.Exceptions;
using Domain.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Domain.Domain.Layers;
using Npgsql;

namespace Service
{
    public class MapService : IMapService
    {
        private readonly DndContext context;
        private readonly IMapper mapper;

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

        public async Task<LayerDto> AddLayer(LayerDto dto, Guid mapId, Guid gameId)
        {
            return await AddLayerToMap(dto, mapId, gameId);
        }

        public async Task<LayerDto> UpdateLayer(LayerDto dto, Guid mapId, Guid gameId)
        {
            var layer = await context.Maps.Where(m => m.GameId == gameId && m.Id == mapId).SelectMany(m => m.Layers)
                .FirstOrDefaultAsync(l => l.Id == dto.Id);

            await layer.Update(dto.Name);

            await context.SaveChangesAsync();

            dto.Name = layer.Name;

            return dto;
        }

        public async Task DeleteLayer(Guid layerId, Guid mapId, Guid gameId)
        {
            var layer = await context.Maps.Where(m => m.GameId == gameId && m.Id == mapId).SelectMany(m => m.Layers)
                .FirstOrDefaultAsync(l => l.Id == layerId);
            context.Layers.Remove(layer);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LayerDto>> GetLayers(Guid mapId, Guid gameId)
        {
            var normalLayers = from l in context.Layers
                       where l.Type == LayerType.Default && l.MapId == mapId && l.Map.GameId == gameId
                       select l;

            var layerDtos = await normalLayers.ProjectTo<LayerDto>(mapper.ConfigurationProvider).ToListAsync();

            return layerDtos.OrderBy(l => l.Order);
        }

        public async Task UpdateLayerOrder(ChangeOrderDto dto, Guid layerId, Guid mapId, Guid gameId)
        {
            var layer = await context.Maps.Where(m => m.GameId == gameId && m.Id == mapId).SelectMany(m => m.Layers)
                .FirstOrDefaultAsync(l => l.Id == layerId);

            if (dto.PreviousPosition > dto.NewPosition)
            {
                var query = @"UPDATE ""Layers""
                SET ""Order"" = (""Order"" + 1)
                WHERE ""Order"" <= @previousPosition
                    AND ""Order"" >= @newPosition
                AND ""MapId"" = @mapId";

                await context.Database.ExecuteSqlCommandAsync(query,
                    new NpgsqlParameter("@previousPosition", dto.PreviousPosition),
                    new NpgsqlParameter("@newPosition", dto.NewPosition),
                    new NpgsqlParameter("@mapId", mapId));
            }
            else if (dto.PreviousPosition < dto.NewPosition)
            {
                var query = @"UPDATE ""Layers""
                SET ""Order"" = (""Order"" - 1)
                WHERE ""Order"" >= @previousPosition
                    AND ""Order"" <= @newPosition
                AND ""MapId"" = @mapId";

                await context.Database.ExecuteSqlCommandAsync(query,
                    new NpgsqlParameter("@previousPosition", dto.PreviousPosition),
                    new NpgsqlParameter("@newPosition", dto.NewPosition),
                    new NpgsqlParameter("@mapId", mapId));
            }

            await layer.UpdateOrder(dto.NewPosition);
            await context.SaveChangesAsync();
        }

        private async Task<LayerDto> AddLayerToMap(LayerDto dto, Guid mapId, Guid gameId)
        {
            var map = await context.Maps.FirstOrDefaultAsync(m => m.GameId == gameId && m.Id == mapId);

            var layer = await map.AddLayer(dto);

            await context.SaveChangesAsync();

            dto.Id = layer.Id;

            return dto;
        }
    }
}
