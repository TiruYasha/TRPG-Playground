using Domain.Domain;
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
        //private IGameService sut;

        //private Mock<IRepository> gameRepository;
        //private Mock<ILogger<GameService>> logger;
        //private Mock<IUserRepository> userRepository;

        //[TestInitialize]
        //public void Initialize()
        //{
        //    gameRepository = new Mock<IRepository>();
        //    logger = new Mock<ILogger<GameService>>();
        //    userRepository = new Mock<IUserRepository>();

        //    sut = new GameService(userRepository.Object, logger.Object);
        //}

        //[TestMethod]
        //public async Task CreateGameCreatesTheGame()
        //{
        //    var gameName = "testing";
        //    var ownerId = new Guid("ce95484d-8ea5-4437-b649-29c4be63ae33");

        //    var user = new User
        //    {
        //        Id = ownerId,
        //        Email = "test@test.nl",
        //    };

        //    userRepository.Setup(s => s.GetUserByIdAsync(ownerId)).ReturnsAsync(user);
        //    gameRepository.Setup(s => s.CreateGameAsync(It.Is<Game>(g => g.Owner == user && g.Name == gameName)))
        //        .Returns(Task.CompletedTask).Verifiable();

        //    var result = await sut.CreateGameAsync(gameName, ownerId);

        //    result.ShouldNotBe(Guid.Empty);
        //    gameRepository.VerifyAll();
        //}

        //[TestMethod]
        //public async Task JoinGameAddsThePlayerToTheGameAndReturnsFalse()
        //{
        //    // Arrange
        //    var userId = new Guid("65a5b497-75b8-4729-9ca7-69152e319380");

        //    var user = new User
        //    {
        //        Id = userId,
        //        Email = "test@test.nl",
        //    };

        //    var game = new Game("name", new User());

        //    userRepository.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync(user);
        //    gameRepository.Setup(s => s.GetGameByIdAsync(game.Id)).ReturnsAsync(game);
        //    gameRepository.Setup(s => s.UpdateGameAsync(It.Is<Game>(g => g.Id == game.Id && g.Players.Any(p => p.UserId == userId)))).Returns(Task.CompletedTask).Verifiable();

        //    // Action
        //    var result = await sut.JoinGameAsync(game.Id, userId);

        //    // Assert
        //    result.ShouldBeFalse();
        //    gameRepository.VerifyAll();
        //}

        //[TestMethod]
        //public async Task JoinGameDoesNothingWhenThePlayerHasAlreadyJoinedAndReturnsFalse()
        //{
        //    // Arrange
        //    var userId = new Guid("65a5b497-75b8-4729-9ca7-69152e319380");

        //    var user = new User
        //    {
        //        Id = userId,
        //        Email = "test@test.nl",
        //    };

        //    var game = new Game("name", new User());

        //    userRepository.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync(user);
        //    gameRepository.Setup(s => s.GetGameByIdAsync(game.Id)).ReturnsAsync(game).Verifiable();

        //    await sut.JoinGameAsync(game.Id, userId);

        //    // Action
        //    var result = await sut.JoinGameAsync(game.Id, userId);

        //    // Assert
        //    result.ShouldBeFalse();
        //    gameRepository.VerifyAll();
        //    gameRepository.Verify(s => s.UpdateGameAsync(It.Is<Game>(g => g.Id == game.Id && g.Players.Any(p => p.UserId == userId))), Times.Once);
        //}

        //[TestMethod]
        //public async Task JoinGameDoesNothingWhenTheOwnerHasAlreadyJoinedAndReturnsTrue()
        //{
        //    // Arrange
        //    var userId = new Guid();

        //    var user = new User
        //    {
        //        Id = userId,
        //        Email = "test@test.nl",
        //    };

        //    var game = new Game("name", new User());

        //    gameRepository.Setup(s => s.GetGameByIdAsync(game.Id)).ReturnsAsync(game).Verifiable();

        //    // Action
        //    var result = await sut.JoinGameAsync(game.Id, userId);

        //    // Assert
        //    result.ShouldBeTrue();
        //    gameRepository.VerifyAll();
        //    gameRepository.Verify(s => s.UpdateGameAsync(It.Is<Game>(g => g.Id == game.Id && g.Players.Any(p => p.UserId == userId))), Times.Never);
        //}
    }
}
