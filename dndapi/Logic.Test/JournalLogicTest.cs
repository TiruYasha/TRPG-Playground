using AutoMapper;
using Database;
using Database.JournalItems;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.JournalItems;
using Moq;
using Repository;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logic.Test
{
    [TestClass]
    public class JournalLogicTest
    {
        private IJournalLogic _sut;

        private Mock<IGameRepository> _gameRepositoryMock;
        private Mock<IMapper> _mapperMock;

        [TestInitialize]
        public void Initialize()
        {
            _gameRepositoryMock = new Mock<IGameRepository>();
            _mapperMock = new Mock<IMapper>();

            _sut = new JournalLogic(_gameRepositoryMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public async Task AddHandoutToGameGivenHandoutThenAddItToTheGame()
        {
            var journal = new Journal
            {
                Id = new Guid()
            };

            var gameToReturn = new Game
            {
                Id = new Guid(),
                Journal = journal
            };

            var journalItemModel = new HandoutModel
            {
                Id = new Guid(),
                Name = "TestHandout",
                Description = "This is a description"
            };

            var journalItem = new Handout
            {
                Id = journalItemModel.Id,
                Name = journalItemModel.Name,
                Description = journalItemModel.Description
            };

            _gameRepositoryMock.Setup(g => g.FindGameByIdAsync(gameToReturn.Id)).Returns(Task.FromResult(gameToReturn)).Verifiable();
            _gameRepositoryMock.Setup(g => g.UpdateGameAsync(It.Is<Game>(q => q.Journal.JournalItems.Count == 1))).Returns(Task.CompletedTask).Verifiable();
            _mapperMock.Setup(m => m.Map<JournalItemModel, JournalItem>(journalItemModel)).Returns(journalItem).Verifiable();
            
            await _sut.AddJournalItemToGameAsync(gameToReturn.Id, journalItemModel);

            _gameRepositoryMock.VerifyAll();
            _mapperMock.VerifyAll();
        }

        [TestMethod]
        public async Task GetAllJournalItemsGivenGameIdReturnsAllJournalItems()
        {
            var journal = new Journal
            {
                Id = new Guid()
            };

            var gameToReturn = new Game
            {
                Id = new Guid(),
                Journal = journal
            };

            var journalItem = new Handout
            {
                Id = new Guid(),
                Name = "TestHandout",
                Description = "This is a description"
            };
            journal.JournalItems.Add(journalItem);

            var journalItems = new List<JournalItemModel>();

            _gameRepositoryMock.Setup(g => g.FindGameByIdAsync(gameToReturn.Id)).Returns(Task.FromResult(gameToReturn)).Verifiable();
            _mapperMock.Setup(m => m.Map<List<JournalItem>, List<JournalItemModel>>(journal.JournalItems)).Returns(journalItems).Verifiable();

            var result = await _sut.GetAllJournalItemsAsync(gameToReturn.Id);

            result.ShouldBe(journalItems);

            _gameRepositoryMock.VerifyAll();
            _mapperMock.VerifyAll();
        }
    }
}
