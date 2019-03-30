using Domain.Domain;
using Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
