using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess;
using Domain;
using Domain.Domain;
using Domain.Domain.JournalItems;
using Domain.MappingProfiles;
using Domain.Mocks;
using Domain.RequestModels.Journal;
using Domain.RequestModels.Journal.JournalItems;
using Domain.ReturnModels.Journal;
using Domain.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;
using Service.Test.Mocks;
using Shouldly;

namespace Service.Test
{
    [TestClass]
    public class JournalServiceTest
    {
        private IJournalService sut;

        private Mock<IRepository> repository;
        private static IMapper _mapper;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            IConfigurationProvider config = new MapperConfiguration(d => d.AddProfile<MyProfile>());
            _mapper = new Mapper(config);
        }

        [TestInitialize]
        public void Initialize()
        {
            repository = new Mock<IRepository>(MockBehavior.Strict);

            sut = new JournalService(repository.Object, _mapper);
        }

        [TestMethod]
        public async Task AddJournalItemToGameAddsTheJournalItemToTheGame()
        {
            // arrange
            var gameId = Guid.NewGuid();
            var addJournalItemModel = new AddJournalItemDto
            {
                JournalItem = new JournalItemDto()
            };

            var journalItem = new JournalItemMock
            {
                Id =  Guid.Empty,
                Name = "test",
                ParentFolderId = Guid.Empty,
                Type = JournalItemType.Folder,
                ImageId = Guid.Empty
            };

            var mockGame1 = new Mock<Game>();
            mockGame1.SetupGet(g => g.Id).Returns(gameId);
            mockGame1.Setup(g => g.AddJournalItemAsync(addJournalItemModel)).ReturnsAsync(journalItem);

            var gameQueryable = new List<Game> { mockGame1.Object };

            var mock = gameQueryable.AsQueryable().BuildMock();

            repository.SetupGet(r => r.Games).Returns(mock.Object);
            repository.Setup(r => r.Commit()).Returns(Task.CompletedTask).Verifiable();

            // act
            var result = await sut.AddJournalItemToGame(addJournalItemModel, gameId);

            // assert
            result.Name.ShouldBe(journalItem.Name);
            result.Id.ShouldBe(journalItem.Id);
            result.ImageId.ShouldBe(journalItem.ImageId);
            result.ParentFolderId.ShouldBe(journalItem.ParentFolderId);
            result.Type.ShouldBe(journalItem.Type);

            repository.VerifyAll();
        }

        [TestMethod]
        public async Task AddJournalItemToGameAddsJournalItemToParent()
        {
            // arrange
            var gameId = Guid.NewGuid();
            var addJournalItemModel = new AddJournalItemDto
            {
                JournalItem = new JournalItemDto(),
                ParentFolderId = Guid.NewGuid()
            };

            var journalItem = new JournalItemMock
            {
                Id = Guid.Empty,
                Name = "test",
                ParentFolderId = Guid.NewGuid(),
                Type = JournalItemType.Folder,
                ImageId = Guid.Empty
            };

            var mockJournalItem = new Mock<JournalFolder>();
            mockJournalItem.SetupGet(s => s.Id).Returns(addJournalItemModel.ParentFolderId.Value);
            mockJournalItem.Setup(s => s.AddJournalItem(addJournalItemModel)).ReturnsAsync(journalItem);

            var journalItemList = new List<JournalFolder> {mockJournalItem.Object};
            var journalFolderQueryable = journalItemList.AsQueryable().BuildMock();

            repository.SetupGet(r => r.JournalFolders).Returns(journalFolderQueryable.Object);
            repository.Setup(r => r.Commit()).Returns(Task.CompletedTask).Verifiable();

            // act
            var result = await sut.AddJournalItemToGame(addJournalItemModel, gameId);

            // assert
            result.Name.ShouldBe(journalItem.Name);
            result.Id.ShouldBe(journalItem.Id);
            result.ImageId.ShouldBe(journalItem.ImageId);
            result.ParentFolderId.ShouldBe(journalItem.ParentFolderId);
            result.Type.ShouldBe(journalItem.Type);

            repository.VerifyAll();
        }
    }
}