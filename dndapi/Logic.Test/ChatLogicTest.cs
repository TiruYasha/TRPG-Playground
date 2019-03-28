using AutoMapper;
using Database;
using Logic.Helpers;
using Logic.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Moq;
using Repository;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Test
{
    [TestClass]
    public class ChatLogicTest
    {
        private IChatLogic _sut;
        private Mock<IGameRepository> _gameRepositoryMock;
        private Mock<IJwtReader> _jwtReaderMock;
        private Mock<ICommandHelper> _commandHelperMock;
        private Mock<IMapper> _mapperMock;

        [TestInitialize]
        public void Initialize()
        {
            _gameRepositoryMock = new Mock<IGameRepository>();
            _jwtReaderMock = new Mock<IJwtReader>();
            _commandHelperMock = new Mock<ICommandHelper>();
            _mapperMock = new Mock<IMapper>();

            _sut = new ChatLogic(_gameRepositoryMock.Object, _jwtReaderMock.Object, _commandHelperMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public async Task GivenMessageFromOwnerThenAddToGameWithReferenceToOwner()
        {
            var message = new ChatMessageModel
            {
                Message = "Hello"
            };

            var gameId = new Guid();
            var userId = new Guid();

            var user = new DndUser
            {
                Id = userId.ToString()
            };

            var gameToReturn = new Game
            {
                Id = gameId,
                Owner = user
            };

            _gameRepositoryMock.Setup(g => g.AddMessageAsync(It.Is<ChatMessage>(
                m => m.Message == message.Message && 
                m.Game.Id == gameId &&
                m.User  == user))).Returns(Task.CompletedTask).Verifiable();
            _gameRepositoryMock.Setup(g => g.FindGameByIdAsync(gameId)).Returns(Task.FromResult(gameToReturn)).Verifiable();
            _jwtReaderMock.Setup(j => j.GetUserId()).Returns(userId);

            await _sut.AddMessageToGameAsync(gameId, message);

            _gameRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public async Task GivenMessageFromParticipantThenAddToGameWithReferenceToParticipant()
        {
            var gameId = new Guid();
            var userId = new Guid();

            var message = new ChatMessageModel
            {
                Message = "Hello"
            };

            var user = new DndUser
            {
                Id = userId.ToString()
            };

            var gameToReturn = new Game
            {
                Id = gameId,
                Participants = new List<Participant>()
            };

            var participant = new Participant
            {
                User = user
            };
            gameToReturn.Participants.Add(participant);

            _gameRepositoryMock.Setup(g => g.AddMessageAsync(It.Is<ChatMessage>(
                m => m.Message == message.Message &&
                m.Game.Id == gameId &&
                m.User == user))).Returns(Task.CompletedTask).Verifiable();
            _gameRepositoryMock.Setup(g => g.FindGameByIdAsync(gameId)).Returns(Task.FromResult(gameToReturn)).Verifiable();
            _jwtReaderMock.Setup(j => j.GetUserId()).Returns(userId);

            await _sut.AddMessageToGameAsync(gameId, message);

            _gameRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public async Task GivenMessageCheckIfItIsCommand()
        {
            var gameId = new Guid();
            var userId = new Guid();

            var message = new ChatMessageModel
            {
                Message = "/roll 1d20"
            };

            var user = new DndUser
            {
                Id = userId.ToString()
            };

            var gameToReturn = new Game
            {
                Id = gameId,
                Participants = new List<Participant>()
            };

            var participant = new Participant
            {
                User = user
            };
            gameToReturn.Participants.Add(participant);

            var rollResult = new NormalRollCommandResult
            {
                Result = 5
            };

            _gameRepositoryMock.Setup(g => g.AddMessageAsync(It.Is<ChatMessage>(
               m => m.CommandResult == rollResult))).Returns(Task.CompletedTask).Verifiable();
            _gameRepositoryMock.Setup(g => g.FindGameByIdAsync(gameId)).Returns(Task.FromResult(gameToReturn)).Verifiable();
            _commandHelperMock.Setup(c => c.CheckIfMessageIsCommand(message.Message)).Returns(true);

            _commandHelperMock.Setup(c => c.RunCommand(message.Message)).Returns(rollResult);

            await _sut.AddMessageToGameAsync(gameId, message);

            _gameRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public async Task GivenGameIdGetAllMessagesForGame()
        {
            var gameId = new Guid();

            var messages = new List<ChatMessage>();
            var message = new ChatMessage
            {
                Id = new Guid(),
                Message = "Hello",
                CommandResult = new NormalRollCommandResult(),
                User = new DndUser
                {
                    Email = "Name"
                }
            };
            messages.Add(message);

            var game = new Game
            {
                Id = gameId,
                Messages = messages
            };

            _gameRepositoryMock.Setup(g => g.FindGameByIdAsync(gameId)).Returns(Task.FromResult(game)).Verifiable();
            _mapperMock.Setup(m => m.Map<List<ChatMessage>, List<ChatMessageModel>>(It.IsAny<List<ChatMessage>>()))
                .Returns(new List<ChatMessageModel>()).Verifiable();

            var result = await _sut.GetChatMessagesForGameAsync(gameId);

            _gameRepositoryMock.VerifyAll();
            _mapperMock.Verify();
        }
    }
}
