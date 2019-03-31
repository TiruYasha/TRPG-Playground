using Domain.Domain;
using Domain.RepositoryInterfaces;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Service.Test.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Test
{
    [TestClass]
    public class GameServiceTest
    {
        private IGameService sut;

        private Mock<IGameRepository> gameRepository;
        private Mock<ILogger<GameService>> logger;
        private Mock<UserManager<User>> userManager;

        [TestInitialize]
        public void Initialize()
        {
            gameRepository = new Mock<IGameRepository>();
            logger = new Mock<ILogger<GameService>>();
            userManager = UserManagerMockFactory.GetUserManagerMock();

            sut = new GameService(gameRepository.Object, userManager.Object, logger.Object);
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

            userManager.SetupGet(s => s.Users).Returns(new List<User>() { user }.AsQueryable());
            gameRepository.Setup(s => s.CreateGameAsync(It.Is<Game>(g => g.Owner == user && g.Name == gameName)))
                .Returns(Task.CompletedTask).Verifiable();

            await sut.CreateGameAsync(gameName, ownerId);

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

            userManager.SetupGet(s => s.Users).Returns(new List<User>() { user }.AsQueryable());
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

            userManager.SetupGet(s => s.Users).Returns(new List<User>() { user }.AsQueryable());
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

            userManager.SetupGet(s => s.Users).Returns(new List<User>() { user }.AsQueryable());
            gameRepository.Setup(s => s.GetGameByIdAsync(game.Id)).ReturnsAsync(game).Verifiable();

            // Action
            await sut.JoinGameAsync(game.Id, userId);

            // Assert
            gameRepository.VerifyAll();
            gameRepository.Verify(s => s.UpdateGameAsync(It.Is<Game>(g => g.Id == game.Id && g.Players.Any(p => p.UserId == userId))), Times.Never);
        }
    }
}
