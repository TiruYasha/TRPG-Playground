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
using Domain.Dto.ReturnDto.Game;

namespace Service
{
    public class GameService : IGameService
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;
        private readonly ILogger<GameService> logger;

        public GameService(IRepository repository, IMapper mapper, ILogger<GameService> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Guid> CreateGameAsync(string gameName, Guid ownerId)
        {
            var game = new Game(gameName, ownerId);

            await repository.Add(game);

            await repository.Commit();

            logger.LogInformation("Game {0} has been created", game.Name);

            return game.Id;
        }

        public async Task<IEnumerable<GameCatalogItemModel>> GetAllGames()
        {
            return await repository.Games.ProjectTo<GameCatalogItemModel>(mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<bool> JoinGameAsync(Guid gameId, Guid userId)
        {
            var game = await repository.Games.Include(g =>g.Players).Include(g => g.Owner).FilterById(gameId).FirstOrDefaultAsync();

            if (await game.HasPlayerJoined(userId))
            {
                return false;
            }

            if (await game.IsOwner(userId))
            {
                return true;
            }

            var user = await repository.Users.FirstOrDefaultAsync(u => u.Id == userId);

            game.Join(user);

            await repository.Commit();

            return false;
        }

        public Task<bool> IsGamePlayerOrOwnerOfGameAsync(Guid userId, Guid activeGameId)
        {
            return repository.Games.AnyAsync(g =>
                g.Id == activeGameId && (g.Players.Any(p => p.UserId == userId) || g.Owner.Id == userId));
        }

        public Task<bool> IsOwnerOfGameAsync(Guid userId, Guid activeGameId)
        {
            return repository.Games.AnyAsync(g => g.Id == activeGameId && g.Owner.Id == userId);
        }

        public async Task<IEnumerable<GetPlayersModel>> GetPlayersAsync(Guid gameId)
        {
            var result = await repository.GamePlayers.Where(g => g.GameId == gameId)
                .ProjectTo<GetPlayersModel>(mapper.ConfigurationProvider)
                .ToListAsync();
            return result;
        }
    }
}
