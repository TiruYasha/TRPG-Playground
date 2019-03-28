using Database;
using Logic.Exceptions;
using Logic.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Moq;
using Repository;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Test
{
    [TestClass]
    public class GameLogicTest
    {
        private IGameLogic _sut;

        private Mock<IGameRepository> _gameRepositoryMock;
        private Mock<UserManager<DndUser>> _userManagerMock;
        private Mock<IJwtReader> _jwtReaderMock;

        [TestInitialize]
        public void Initialize()
        {
            _gameRepositoryMock = new Mock<IGameRepository>(MockBehavior.Strict);
            _userManagerMock = MockHelper.MockUserManager<DndUser>();
            _jwtReaderMock = new Mock<IJwtReader>(MockBehavior.Strict);

            _sut = new GameLogic(_gameRepositoryMock.Object, _userManagerMock.Object, _jwtReaderMock.Object);
        }

        [TestMethod]
        public async Task GivenGameModelThenCreateGameWithUserAndReturnGameModel()
        {
            var model = new GameModel
            {
                Name = "Goblins"
            };

            var user = new DndUser();
            var userId = new Guid("a395a483-cf49-4db5-81a2-e4faa5ff5701");

            _gameRepositoryMock.Setup(g => g.InsertGameAsync(It.Is<Game>(f => f.Name == model.Name && f.Owner == user))).Returns(Task.CompletedTask).Verifiable();
            _userManagerMock.Setup(u => u.FindByIdAsync(userId.ToString())).Returns(Task.FromResult(user)).Verifiable();
            _jwtReaderMock.Setup(j => j.GetUserId()).Returns(userId).Verifiable();

            var result = await _sut.CreateGameAsync(model);

            result.Name.ShouldBe(model.Name);
            _gameRepositoryMock.Verify();
            _userManagerMock.Verify();
            _jwtReaderMock.Verify();
        }

        [TestMethod]
        public void GivenGameModelThenJoinGameWithAddingParticipant()
        {
            var model = new GameModel
            {
                Id = new Guid("a395a483-cf49-4db5-81a2-e4faa5ff5703")
            };

            var user = new DndUser();
            var userId = new Guid("a395a483-cf49-4db5-81a2-e4faa5ff5701");

            var game = new Game
            {
                Id = model.Id,
                Owner = user
            };

            _gameRepositoryMock.Setup(g => g.FindGameByIdAsync(model.Id)).Returns(Task.FromResult(game)).Verifiable();
            _gameRepositoryMock.Setup(g => g.UpdateGameAsync(It.Is<Game>(m => m.Participants.First().User == user && m.Participants.First().Game == game))).Verifiable();
            
            _jwtReaderMock.Setup(j => j.GetUserId()).Returns(userId).Verifiable();
            _userManagerMock.Setup(u => u.FindByIdAsync(userId.ToString())).Returns(Task.FromResult(user)).Verifiable();

            _sut.JoinGameAsync(model);

            _gameRepositoryMock.VerifyAll();
            _jwtReaderMock.Verify();
            _userManagerMock.Verify();
        }

        [TestMethod]
        public void GivenGameModelAndOwnerThenJoinGameWithoutAddingParticipant()
        {
            var model = new GameModel
            {
                Id = new Guid("a395a483-cf49-4db5-81a2-e4faa5ff5703")
            };

            var user = new DndUser();
            var userId = new Guid("a395a483-cf49-4db5-81a2-e4faa5ff5701");

            var game = new Game
            {
                Id = model.Id,
                Owner = user
            };

            _gameRepositoryMock.Setup(g => g.FindGameByIdAsync(model.Id)).Returns(Task.FromResult(game)).Verifiable();
            _jwtReaderMock.Setup(j => j.GetUserId()).Returns(userId).Verifiable();

            _sut.JoinGameAsync(model);

            _gameRepositoryMock.Verify();
            _jwtReaderMock.Verify();
        }

        [TestMethod]
        public async Task GivenGameModelAndOwnerThenJoinNonExistingGameReturnsGameNotFoundException()
        {
            var model = new GameModel
            {
                Id = Guid.NewGuid()
            };

            var user = new DndUser();
            var userId = new Guid("a395a483-cf49-4db5-81a2-e4faa5ff5701");

            _gameRepositoryMock.Setup(g => g.FindGameByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult((Game)null)).Verifiable();
            _jwtReaderMock.Setup(j => j.GetUserId()).Returns(userId).Verifiable();

            var exception = await Should.ThrowAsync<GameNotFoundException>(() => _sut.JoinGameAsync(model));


            exception.Message.ShouldBe("The game you are trying to join does not exist");
            _gameRepositoryMock.Verify();
            _jwtReaderMock.Verify();
        }

        [TestMethod]
        public void GetGamesReturnsAllGames()
        {
            var games = new List<Game>();
            var game1 = new Game
            {
                Id = new Guid(),
                Name = "Hello"
            };
            games.Add(game1);

            _gameRepositoryMock.Setup(g => g.GetAllGames()).Returns(games).Verifiable();

            var result = _sut.GetAllGames();

            result.Count.ShouldBe(1);
            result.First().Id.ShouldBe(game1.Id);
            result.First().Name.ShouldBe(game1.Name);
        }
    }
}
