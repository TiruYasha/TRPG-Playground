using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestApi.Utilities;
using Shouldly;
using System;
using System.Net;
using System.Threading.Tasks;
using Domain.Dto.RequestDto.Game;
using Domain.Dto.ReturnDto.Game;
using Domain.Dto.Shared;
using Microsoft.AspNetCore.SignalR;
using RestApi.Hubs;
using Domain.Events;

namespace RestApi.Test
{
    [TestClass]
    public class GameControllerTest
    {
        private GameController sut;

        private Mock<IGameService> gameService;
        private Mock<IJwtReader> jwtReader;
        private Mock<IHubContext<GameHub>> hubContext;

        [TestInitialize]
        public void Initialize()
        {
            gameService = new Mock<IGameService>();
            jwtReader = new Mock<IJwtReader>();
            hubContext = new Mock<IHubContext<GameHub>>(MockBehavior.Strict);

            sut = new GameController(gameService.Object, jwtReader.Object, hubContext.Object);
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

        [TestMethod]
        public async Task AddMapToPlayAreaReturnsOkWithMap()
        {
            // Arrange
            var mapDto = new MapDto();
            var mapToReturn = new MapDto();
            var gameId = Guid.NewGuid();

            jwtReader.Setup(j => j.GetGameId()).Returns(gameId);

            gameService.Setup(p => p.AddMap(mapDto, gameId)).ReturnsAsync(mapToReturn);

            // Act
            var result = await sut.AddMapToPlayArea(mapDto) as OkObjectResult;

            // Assert
            result.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            result.Value.ShouldBe(mapToReturn);
        }
    }
}
