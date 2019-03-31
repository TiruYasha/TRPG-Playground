﻿using Domain.Domain;
using Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public IQueryable<Game> GetAllGames()
        {
            return context.Games;
        }

        public async Task<Game> GetGameByIdAsync(Guid gameId)
        {
            return await context.Games.FirstOrDefaultAsync(g => g.Id == gameId);
        }

        public User GetUserById(Guid id)
        {
            return context.Users.FirstOrDefault(u => u.Id == id);
        }

        public async Task UpdateGameAsync(Game game)
        {
            context.Entry(game.Owner).State = EntityState.Unchanged;
            context.Games.Update(game);
            await context.SaveChangesAsync();
        }
    }
}
