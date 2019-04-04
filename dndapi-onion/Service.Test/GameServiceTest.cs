using Domain.Domain;
using Domain.RepositoryInterfaces;
using Domain.ServiceInterfaces;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Test
{
    [TestClass]
    public class GameServiceTest
    {
        private IGameService sut;

        private Mock<IGameRepository> gameRepository;
        private Mock<ILogger<GameService>> logger;
        private Mock<IUserRepository> userRepository;

        [TestInitialize]
        public void Initialize()
        {
            gameRepository = new Mock<IGameRepository>();
            logger = new Mock<ILogger<GameService>>();
            userRepository = new Mock<IUserRepository>();

            sut = new GameService(gameRepository.Object, userRepository.Object, logger.Object);
        }

        [TestMethod]
        public async Task CreateGameCreatesTheGame()
        {
            var gameName = "testing";
            var ownerId = new Guid("ce95484d-8ea5-4437-b649-29c4be63ae33");

            var user = new User
            {
                Id = ownerId,
                Email = "test@test.nl",
            };

            userRepository.Setup(s => s.GetUserByIdAsync(ownerId)).ReturnsAsync(user);
            gameRepository.Setup(s => s.CreateGameAsync(It.Is<Game>(g => g.Owner == user && g.Name == gameName)))
                .Returns(Task.CompletedTask).Verifiable();

            var result = await sut.CreateGameAsync(gameName, ownerId);

            result.ShouldNotBe(Guid.Empty);
            gameRepository.VerifyAll();
        }

        [TestMethod]
        public async Task JoinGameAddsThePlayerToTheGame()
        {
            // Arrange
            var userId = new Guid("65a5b497-75b8-4729-9ca7-69152e319380");

            var user = new User
            {
                Id = userId,
                Email = "test@test.nl",
            };
             
            var game = new Game("name", new User());

            userRepository.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync(user);
            gameRepository.Setup(s => s.GetGameByIdAsync(game.Id)).ReturnsAsync(game);
            gameRepository.Setup(s => s.UpdateGameAsync(It.Is<Game>(g => g.Id == game.Id && g.Players.Any(p => p.UserId == userId)))).Returns(Task.CompletedTask).Verifiable();

            // Action
            await sut.JoinGameAsync(game.Id, userId);

            // Assert
            gameRepository.VerifyAll();
        }

        [TestMethod]
        public async Task JoinGameDoesNothingWhenThePlayerHasAlreadyJoined()
        {
            // Arrange
            var userId = new Guid("65a5b497-75b8-4729-9ca7-69152e319380");

            var user = new User
            {
                Id = userId,
                Email = "test@test.nl",
            };

            var game = new Game("name", new User());

            userRepository.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync(user);
            gameRepository.Setup(s => s.GetGameByIdAsync(game.Id)).ReturnsAsync(game).Verifiable();

            await sut.JoinGameAsync(game.Id, userId);

            // Action
            await sut.JoinGameAsync(game.Id, userId);

            // Assert
            gameRepository.VerifyAll();
            gameRepository.Verify(s => s.UpdateGameAsync(It.Is<Game>(g => g.Id == game.Id && g.Players.Any(p => p.UserId == userId))), Times.Once);
        }

        [TestMethod]
        public async Task JoinGameDoesNothingWhenTheOwnerHasAlreadyJoined()
        {
            // Arrange
            var userId = new Guid();

            var user = new User
            {
                Id = userId,
                Email = "test@test.nl",
            };

            var game = new Game("name", new User());

            userRepository.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync(user);
            gameRepository.Setup(s => s.GetGameByIdAsync(game.Id)).ReturnsAsync(game).Verifiable();

            // Action
            await sut.JoinGameAsync(game.Id, userId);

            // Assert
            gameRepository.VerifyAll();
            gameRepository.Verify(s => s.UpdateGameAsync(It.Is<Game>(g => g.Id == game.Id && g.Players.Any(p => p.UserId == userId))), Times.Never);
        }
    }
}
