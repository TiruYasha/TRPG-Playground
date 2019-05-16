using Domain.Domain;
using System;
using System.Linq;

namespace DataAccess
{
    public static class GameQueries
    {
        public static IQueryable<Game> FilterByGameId(
            this IQueryable<Game> queryable,
            Guid gameId)
        {
            return queryable.Where(g => g.Id == gameId);
        }

        //public async Task CreateGameAsync(Game game)
        //{
        //    context.Entry(game.Owner).State = EntityState.Unchanged;
        //    await context.Games.AddAsync(game);

        //    await context.SaveChangesAsync();
        //}

        //public IQueryable<Game> GetAllGames()
        //{
        //    return context.Games;
        //}

        //public async Task<Game> GetGameByIdAsync(Guid gameId)
        //{
        //    return await context.Games.FirstOrDefaultAsync(g => g.Id == gameId);
        //}

        //public async Task UpdateGameAsync(Game game)
        //{
        //    context.Games.Update(game);


        //    await context.SaveChangesAsync();
        //}

        //public Task<bool> IsGamePlayerOrOwnerOfGameAsync(Guid userId, Guid activeGameId)
        //{
        //    return context.Games.AnyAsync(g => g.Id == activeGameId && (g.Players.Any(p => p.UserId == userId) || g.Owner.Id == userId));
        //}

        //public Task<bool> IsOwnerOfGameAsync(Guid userId, Guid activeGameId)
        //{
        //    return context.Games.AnyAsync(g => g.Id == activeGameId && g.Owner.Id == userId);
        //}

        //public Task<IQueryable<GamePlayer>> GetPlayersFromGameAsync(Guid gameId)
        //{
        //    return Task.Run(() =>
        //    {
        //        return context.GamePlayers.Where(g => g.GameId == gameId);
        //    });
        //}
    }
}
