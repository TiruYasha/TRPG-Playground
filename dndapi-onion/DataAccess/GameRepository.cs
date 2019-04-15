using Domain.Domain;
using Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public class GameRepository : IGameRepository
    {
        private readonly DndContext context;

        public GameRepository(DbContextOptions<DndContext> options)
        {
            this.context = new DndContext(options);
        }

        public async Task CreateGameAsync(Game game)
        {
            context.Entry(game.Owner).State = EntityState.Unchanged;
            await context.Games.AddAsync(game);

            await context.SaveChangesAsync();
        }

        public IQueryable<Game> GetAllGames()
        {
            return context.Games;
        }

        public async Task<Game> GetGameByIdAsync(Guid gameId)
        {
            return await context.Games.FirstOrDefaultAsync(g => g.Id == gameId);
        }

        public async Task UpdateGameAsync(Game game)
        {
            context.Games.Update(game);
            
            
            await context.SaveChangesAsync();
        }

        public Task<bool> IsGamePlayerOrOwnerOfGameAsync(Guid userId, Guid activeGameId)
        {
            return context.Games.AnyAsync(g => g.Id == activeGameId && (g.Players.Any(p => p.UserId == userId) || g.Owner.Id == userId));
        }

        public Task<bool> IsOwnerOfGameAsync(Guid userId, Guid activeGameId)
        {
            return context.Games.AnyAsync(g => g.Id == activeGameId && g.Owner.Id == userId);
        }

        public Task<IQueryable<GamePlayer>> GetPlayersFromGameAsync(Guid gameId)
        {
            return Task.Run(() =>
            {
                return context.GamePlayers.Where(g => g.GameId == gameId);
            });
        }
    }
}
