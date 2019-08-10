using AutoMapper;
using DataAccess;
using Domain.Domain.PlayArea;
using Domain.Dto.Shared;
using Domain.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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
    }
}
