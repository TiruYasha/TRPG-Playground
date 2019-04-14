using AutoMapper;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestApi.Models.Game;
using RestApi.Utilities;
using Shouldly;
using System;
using System.Threading.Tasks;

namespace RestApi.Test
{
    [TestClass]
    public class GameControllerTest
    {
        private GameController sut;

        private Mock<IGameService> gameService;
        private Mock<IJwtReader> jwtReader;
        private Mock<IMapper> mapper;

        [TestInitialize]
        public void Initialize()
        {
            gameService = new Mock<IGameService>();
            jwtReader = new Mock<IJwtReader>();
            mapper = new Mock<IMapper>();

            sut = new GameController(gameService.Object, jwtReader.Object, mapper.Object);
        }

        [TestMethod]
        public async Task CreateGameAsyncReturnsOkResult()
        {
            // Arrange
            var gameName = "test";
            var userId = new Guid();
            var gameId = new Guid();
            var model = new CreateGameModel
            {
                Name = gameName
            };

            jwtReader.Setup(s => s.GetUserId()).Returns(userId);
            gameService.Setup(s => s.CreateGameAsync(gameName, userId)).ReturnsAsync(gameId).Verifiable();

            // Action
            var result = await sut.CreateGameAsync(model) as OkObjectResult;

            // Assert
            result.ShouldBeOfType<OkObjectResult>();
            result.Value.ShouldBe(gameId);
            gameService.VerifyAll();
        }

        [TestMethod]
        public async Task CreateGameAsyncReturnsBadRequestOnException()
        {
            // Arrange
            var errorMessage = "argumentException";
            var model = new CreateGameModel
            {
                Name = "test"
            };

            jwtReader.Setup(s => s.GetUserId()).Throws(new ArgumentException(errorMessage));

            // Action
            var result = await sut.CreateGameAsync(model) as BadRequestObjectResult;

            // Assert
            result.ShouldBeOfType<BadRequestObjectResult>();
            result.Value.ShouldBe(errorMessage);
        }

        [TestMethod]
        public async Task JoinGameAsyncReturnsOkResult()
        {
            // Arrange
            var userId = new Guid();
            var gameId = Guid.NewGuid();

            jwtReader.Setup(s => s.GetUserId()).Returns(userId);
            jwtReader.Setup(s => s.GetGameId()).Returns(gameId);
            gameService.Setup(s => s.JoinGameAsync(gameId, userId)).ReturnsAsync(true);

            // Action
            var result = await sut.JoinGameAsync() as OkObjectResult;

            // Assert
            result.ShouldBeOfType<OkObjectResult>();
            result.Value.ShouldBe(true);
        }

        [TestMethod]
        public async Task JoinGameAsyncReturnsBadRequestOnException()
        {
            // Arrange
            var errorMessage = "argumentException";
            var model = new JoinGameModel
            {
                GameId = new Guid(),
            };

            jwtReader.Setup(s => s.GetUserId()).Throws(new ArgumentException(errorMessage));

            // Action
            var result = await sut.JoinGameAsync() as BadRequestObjectResult;

            // Assert
            result.ShouldBeOfType<BadRequestObjectResult>();
            result.Value.ShouldBe(errorMessage);
        }
    }
}
