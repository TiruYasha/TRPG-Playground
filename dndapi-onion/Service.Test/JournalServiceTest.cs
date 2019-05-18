using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Domain.Domain;
using Domain.Domain.JournalItems;
using Domain.MappingProfiles;
using Domain.Mocks;
using Domain.RequestModels.Journal;
using Domain.RequestModels.Journal.JournalItems;
using Domain.ServiceInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;
using Shouldly;

namespace Service.Test
{
    [TestClass]
    public class JournalServiceTest
    {
        private static IMapper _mapper;

        private Mock<IRepository> repository;
        private IJournalService sut;

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
                Id = Guid.Empty,
                Name = "test",
                ParentFolderId = Guid.Empty,
                Type = JournalItemType.Folder,
                ImageId = Guid.Empty
            };

            var mockGame1 = new Mock<Game>();
            mockGame1.SetupGet(g => g.Id).Returns(gameId);
            mockGame1.Setup(g => g.AddJournalItemAsync(addJournalItemModel)).ReturnsAsync(journalItem);

            var gameQueryable = new List<Game> {mockGame1.Object};

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

            // Journalitem mock

            var mockJournalItem = new Mock<JournalFolder>();
            mockJournalItem.SetupGet(s => s.Id).Returns(addJournalItemModel.ParentFolderId.Value);
            mockJournalItem.Setup(s => s.AddJournalItem(addJournalItemModel, gameId)).ReturnsAsync(journalItem);

            var journalItemList = new List<JournalFolder> {mockJournalItem.Object};
            var journalFolderQueryable = journalItemList.AsQueryable().BuildMock();

            repository.SetupGet(r => r.JournalFolders).Returns(journalFolderQueryable.Object);
            

      

            // Others
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
        public async Task GetJournalItemsForParentFolderIdForOwner()
        {
            // arrange
            var gameId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var parentFolderId = Guid.NewGuid();

            var journalItem = new JournalFolder
            {
                Id = Guid.Empty,
                Name = "test",
                ParentFolderId = parentFolderId,
                Type = JournalItemType.Folder,
                ImageId = Guid.Empty
            };

            var journalItemList = new List<JournalFolder> {journalItem};
            var journalFolderQueryable = journalItemList.AsQueryable().BuildMock();

            repository.SetupGet(r => r.JournalItems).Returns(journalFolderQueryable.Object);

            // Game mock
            var mockGame1 = new Mock<Game>();
            mockGame1.SetupGet(g => g.Id).Returns(gameId);
            mockGame1.SetupGet(g => g.Owner.Id).Returns(userId);

            var gameQueryable = new List<Game> { mockGame1.Object };

            var mock = gameQueryable.AsQueryable().BuildMock();

            repository.SetupGet(r => r.Games).Returns(mock.Object);

            // act
            var result = await sut.GetJournalItemsForParentFolderId(userId, gameId, parentFolderId);

            // assert
            var resultObject = result.First();
            result.Count().ShouldBe(1);
            resultObject.Name.ShouldBe(journalItem.Name);
            resultObject.Id.ShouldBe(journalItem.Id);
            resultObject.ImageId.ShouldBe(journalItem.ImageId);
            resultObject.ParentFolderId.ShouldBe(journalItem.ParentFolderId);
            resultObject.Type.ShouldBe(journalItem.Type);
        }

        [TestMethod]
        public async Task GetJournalItemsForNullParentFolderId()
        {
            // arrange
            var gameId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var journalItem = new JournalFolder
            {
                Id = Guid.Empty,
                Name = "test",
                Type = JournalItemType.Folder,
                ImageId = Guid.Empty,
                GameId = gameId
            };

            var journalItem2 = new JournalFolder
            {
                Id = Guid.Empty,
                Name = "test2",
                Type = JournalItemType.Folder,
                ImageId = Guid.Empty,
                GameId = Guid.Empty
            };

            var journalItemList = new List<JournalFolder> { journalItem, journalItem2 };
            var journalFolderQueryable = journalItemList.AsQueryable().BuildMock();

            repository.SetupGet(r => r.JournalItems).Returns(journalFolderQueryable.Object);

            // Game mock
            var mockGame1 = new Mock<Game>();
            mockGame1.SetupGet(g => g.Id).Returns(gameId);
            mockGame1.SetupGet(g => g.Owner.Id).Returns(userId);
            

            var gameQueryable = new List<Game> { mockGame1.Object };

            var mock = gameQueryable.AsQueryable().BuildMock();

            repository.SetupGet(r => r.Games).Returns(mock.Object);

            // act
            var result = await sut.GetJournalItemsForParentFolderId(userId, gameId, null);

            // assert
            var resultObject = result.First();
            result.Count().ShouldBe(1);
            resultObject.Name.ShouldBe(journalItem.Name);
            resultObject.Id.ShouldBe(journalItem.Id);
            resultObject.ImageId.ShouldBe(journalItem.ImageId);
            resultObject.ParentFolderId.ShouldBe(journalItem.ParentFolderId);
            resultObject.Type.ShouldBe(journalItem.Type);
        }
    }
}