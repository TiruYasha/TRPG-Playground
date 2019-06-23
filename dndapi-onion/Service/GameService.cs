using AutoMapper.QueryableExtensions;
using DataAccess;
using Domain.Domain;
using Domain.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Domain.Dto.RequestDto;
using Domain.Dto.ReturnDto.Game;
using Domain.Dto.Shared;
using Domain.Exceptions;

namespace Service
{
    public class GameService : IGameService
    {
        private readonly DndContext context;
        private readonly IMapper mapper;
        private readonly ILogger<GameService> logger;

        public GameService(DndContext context, IMapper mapper, ILogger<GameService> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Guid> CreateGameAsync(string gameName, Guid ownerId)
        {
            var game = new Game(gameName, ownerId);

            await context.AddAsync(game);

            await context.SaveChangesAsync();

            logger.LogInformation("Game {0} has been created", game.Name);

            return game.Id;
        }

        public async Task<IEnumerable<GameCatalogItemModel>> GetAllGames()
        {
            return await context.Games.ProjectTo<GameCatalogItemModel>(mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<bool> JoinGameAsync(Guid gameId, Guid userId)
        {
            var game = await context.Games.Include(g =>g.Players).Include(g => g.Owner).FilterById(gameId).FirstOrDefaultAsync();

            if (await game.HasPlayerJoined(userId))
            {
                return false;
            }

            if (await game.IsOwner(userId))
            {
                return true;
            }

            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            await game.Join(user);

            await context.SaveChangesAsync();

            return false;
        }

        public Task<bool> IsGamePlayerOrOwnerOfGameAsync(Guid userId, Guid activeGameId)
        {
            return context.Games.AnyAsync(g =>
                g.Id == activeGameId && (g.Players.Any(p => p.UserId == userId) || g.Owner.Id == userId));
        }

        public Task<bool> IsOwnerOfGameAsync(Guid userId, Guid activeGameId)
        {
            return context.Games.AnyAsync(g => g.Id == activeGameId && g.Owner.Id == userId);
        }

        public async Task<IEnumerable<GetPlayersModel>> GetPlayersAsync(Guid gameId)
        {
            var result = await context.GamePlayers.Where(g => g.GameId == gameId)
                .ProjectTo<GetPlayersModel>(mapper.ConfigurationProvider)
                .ToListAsync();
            return result;
        }

        public async Task<MapDto> AddMap(AddMapDto dto, Guid gameId)
        {
            var game = await context.Games.FilterById(gameId).FirstOrDefaultAsync();

            if (game == null)
            {
                throw new NotFoundException("The game can not be found");
            }

            var map = await game.AddMap(dto);

            await context.SaveChangesAsync();

            return mapper.Map<Map, MapDto>(map);
        }

        public async Task<IEnumerable<MapDto>> GetMaps(Guid gameId)
        {
            var query = context.Games
                .Include(p => p.Maps)
                .FilterById(gameId)
                .SelectMany(p => p.Maps);

            return await query.ProjectTo<MapDto>(mapper.ConfigurationProvider).ToListAsync();
        }
    }
}
