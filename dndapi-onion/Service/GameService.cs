﻿using Domain.Domain;
using Domain.RepositoryInterfaces;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Identity;
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
            User user = GetUserById(ownerId);

            var game = new Game(gameName, user);

            await gameRepository.CreateGameAsync(game);

            logger.LogInformation("Game {0} has been created", game.Name);
        }

        public IList<Game> GetAllGames()
        {
            return gameRepository.GetAllGames()
                .Select(g => new Game { Id =  g.Id, Name = g.Name }).ToList();
        }

        public async Task JoinGameAsync(Guid gameId, Guid userId)
        {
            var user = gameRepository.GetUserById(userId);//GetUserById(userId);

            var game = await gameRepository.GetGameByIdAsync(gameId);

            if (game.HasPlayerJoined(userId) || game.IsOwner(userId))
            {
                return;
            }

            game.Join(user);

            await gameRepository.UpdateGameAsync(game);
        }

        private User GetUserById(Guid ownerId)
        {
            return userManager.Users.FirstOrDefault(u => u.Id == ownerId);
        }
    }
}
