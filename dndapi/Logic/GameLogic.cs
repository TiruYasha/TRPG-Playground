using Database;
using Logic.Exceptions;
using Logic.Utilities;
using Microsoft.AspNetCore.Identity;
using Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logic
{
    public class GameLogic : IGameLogic
    {
        private readonly IGameRepository _gameRepository;
        private readonly UserManager<DndUser> _userManager;
        private readonly IJwtReader _jwtReader;

        public GameLogic(IGameRepository gameRepository, UserManager<DndUser> userManager, IJwtReader jwtReader)
        {
            _gameRepository = gameRepository;
            _userManager = userManager;
            _jwtReader = jwtReader;
        }

        public async Task<GameModel> CreateGameAsync(GameModel model)
        {
            var userId = _jwtReader.GetUserId();

            var user = await _userManager.FindByIdAsync(userId.ToString());

            var game = new Game
            {
                Name = model.Name,
                Owner = user
            };

            await _gameRepository.InsertGameAsync(game);

            model.Id = game.Id;

            return model;
        }

        public List<GameModel> GetAllGames()
        {
            var games = _gameRepository.GetAllGames();

            return parseGamesToGameModelList(games);
        }

        private List<GameModel> parseGamesToGameModelList(List<Game> games)
        {
            var gameModels = new List<GameModel>();

            foreach(var game in games)
            {
                var gameModel = new GameModel
                {
                    Id = game.Id,
                    Name = game.Name
                };

                gameModels.Add(gameModel);
            }

            return gameModels;
        }

        public async Task JoinGameAsync(GameModel model)
        {
            var game = await _gameRepository.FindGameByIdAsync(model.Id);

            var userId = _jwtReader.GetUserId();

            if(game == null)
            {
                throw new GameNotFoundException("The game you are trying to join does not exist");
            }

            if(game.Owner.Id != userId.ToString() && !game.Participants.Any(g => g.User?.Id == userId.ToString()))
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());

                var participant = new Participant
                {
                    Id = Guid.NewGuid(),
                    User = user,
                    Game = game
                };

                game.Participants.Add(participant);
                await _gameRepository.UpdateGameAsync(game);
            }
        }
    }
}
