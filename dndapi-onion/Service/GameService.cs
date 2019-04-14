using Domain.Domain;
using Domain.RepositoryInterfaces;
using Domain.ServiceInterfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service
{
    public class GameService : IGameService
    {
        private readonly IGameRepository gameRepository;
        private readonly IUserRepository userRepository;
        private readonly ILogger<GameService> logger;

        public GameService(IGameRepository gameRepository, IUserRepository userRepository, ILogger<GameService> logger)
        {
            this.gameRepository = gameRepository;
            this.userRepository = userRepository;
            this.logger = logger;
        }

        public async Task<Guid> CreateGameAsync(string gameName, Guid ownerId)
        {
            var user = await userRepository.GetUserByIdAsync(ownerId);

            var game = new Game(gameName, user);

            await gameRepository.CreateGameAsync(game);

            logger.LogInformation("Game {0} has been created", game.Name);

            return game.Id;
        }

        public IList<Game> GetAllGames()
        {
            return gameRepository.GetAllGames()
                .Select(g => new Game(g.Name, g.Id)).ToList();
        }

        public async Task<bool> JoinGameAsync(Guid gameId, Guid userId)
        {
            var game = await gameRepository.GetGameByIdAsync(gameId);

            if (game.HasPlayerJoined(userId))
            {
                return false;
            }
            else if (game.IsOwner(userId))
            {
                return true;
            }

            var user = await userRepository.GetUserByIdAsync(userId);

            game.Join(user);

            await gameRepository.UpdateGameAsync(game);

            return false;
        }

        public Task<bool> IsGamePlayerOrOwnerOfGameAsync(Guid userId, Guid activeGameId)
        {
            return gameRepository.IsGamePlayerOrOwnerOfGameAsync(userId, activeGameId);
        }

        public Task<bool> IsOwnerOfGameAsync(Guid userId, Guid activeGameId)
        {
            return gameRepository.IsOwnerOfGameAsync(userId, activeGameId);
        }
    }
}
