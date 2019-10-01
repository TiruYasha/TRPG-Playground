using AutoMapper;
using DataAccess;
using Domain.Domain.PlayArea;
using Domain.Dto.Shared;
using Domain.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Service
{
    public class LayerService : ILayerService
    {
        private readonly DndContext context;
        private readonly IMapper mapper;

        public LayerService(DndContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<TokenDto> AddTokenToLayer(TokenDto dto, Guid gameId, Guid userId, Guid layerId)
        {
            var layer = await context.Layers.FirstOrDefaultAsync(l => l.Id == layerId);

            var token = await layer.AddToken(dto);
            await context.SaveChangesAsync();

            return mapper.Map<Token, TokenDto>(token);
        }

        public async Task DeleteToken(Guid tokenId, Guid gameId, Guid layerId)
        {
            var token = await context.Tokens.FirstOrDefaultAsync(t => t.Id == tokenId && t.LayerId == layerId);
            context.Tokens.Remove(token);
            await context.SaveChangesAsync();
        }

        public async Task ToggleVisibleForPlayers(Guid gameId, Guid layerId)
        {
            var layer = await context.Layers.FirstOrDefaultAsync(l => l.Id == layerId && l.Map.GameId == gameId);

            await layer.ToggleVisibleToPlayers();

            await context.SaveChangesAsync();
        }

        public async Task ToggleVisible(Guid gameId, Guid layerId)
        {
            var layer = await context.Layers.FirstOrDefaultAsync(l => l.Id == layerId && l.Map.GameId == gameId);

            await layer.ToggleVisible();

            await context.SaveChangesAsync();
        }

        public async Task MoveToken(MoveTokenDto dto, Guid gameId, Guid userId, Guid layerId)
        {
            var token = await context.Tokens.FirstOrDefaultAsync(t => t.Id == dto.Id && t.LayerId == layerId);

            await token.Move(dto.X, dto.Y);

            await context.SaveChangesAsync();
        }
    }
}
