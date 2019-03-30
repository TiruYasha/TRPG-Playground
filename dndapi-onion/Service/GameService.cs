using Domain.Domain;
using Domain.RepositoryInterfaces;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Service
{
    public class GameService : IGameService
    {
        private readonly IGameRepository gameRepository;
        private readonly UserManager<User> userManager;
        private readonly ILogger<GameService> logger;

        public GameService(IGameRepository gameRepository, UserManager<User> userManager, ILogger<GameService> logger)
        {
            this.gameRepository = gameRepository;
            this.userManager = userManager;
            this.logger = logger;
        }

        public async Task CreateGameAsync(string gameName, Guid ownerId)
        {
            var user = userManager.Users.FirstOrDefault(u => u.Id == ownerId);

            var game = new Game(gameName, user);

            await gameRepository.CreateGameAsync(game);

            logger.LogInformation("Game {0} has been created", game.Name);
        }
    }
}
