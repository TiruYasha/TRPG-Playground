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
using Domain.ReturnModels.Game;

namespace Service
{
    public class GameService : Service, IGameService
    {
        private readonly IMapper mapper;
        private readonly ILogger<GameService> logger;

        public GameService(DbContextOptions<DndContext> options, IMapper mapper, ILogger<GameService> logger) : base(options)
        {
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Guid> CreateGameAsync(string gameName, Guid ownerId)
        {
            var user = await context.Users.FindAsync(ownerId);

            var game = new Game(gameName, user);

            context.Entry(game.Owner).State = EntityState.Unchanged;
            await context.Games.AddAsync(game);

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
            var game = await context.Games.Include(g =>g.Players).Include(g => g.Owner).FilterByGameId(gameId).FirstOrDefaultAsync();

            if (game.HasPlayerJoined(userId))
            {
                return false;
            }

            if (game.IsOwner(userId))
            {
                return true;
            }

            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            game.Join(user);

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
    }
}
