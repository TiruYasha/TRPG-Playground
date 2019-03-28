using Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class GameRepository : IGameRepository, IDisposable
    {
        private readonly DndContext _db;

        public GameRepository(DndContext db)
        {
            _db = db;
        }

        public async Task InsertGameAsync(Game game)
        {
            game.Id = Guid.NewGuid();
            await _db.Games.AddAsync(game);
            _db.SaveChanges();
        }

        public List<Game> GetAllGames()
        {
            return _db.Games.ToList();
        }

        public async Task<Game> FindGameByIdAsync(Guid gameId)
        {
            return await _db.Games.FirstOrDefaultAsync(g => g.Id == gameId);
        }

        public async Task UpdateGameAsync(Game game)
        {
            _db.Update(game);
            await _db.SaveChangesAsync();
        }
       
        public async Task AddMessageAsync(ChatMessage message)
        {
            _db.ChatMessages.Add(message);
            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
            }
        }

    }
}
