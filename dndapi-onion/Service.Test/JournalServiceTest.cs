using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess;
using Domain;
using Domain.Config;
using Domain.Domain;
using Domain.Domain.JournalItems;
using Domain.DomainInterfaces;
using Domain.Dto.RequestDto.Journal;
using Domain.Dto.Shared;
using Domain.Exceptions;
using Domain.MappingProfiles;
using Domain.Mocks;
using Domain.ServiceInterfaces;
using Domain.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
        private Mock<IOptions<FileStorageConfig>> fileStorageConfigOptions;
        private Mock<ImageProcesser> processor;

        private DndContext context;

        private FileStorageConfig fileStorageConfig;
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
            fileStorageConfigOptions = new Mock<IOptions<FileStorageConfig>>();
            processor = new Mock<ImageProcesser>();

            fileStorageConfig = new FileStorageConfig
            {
                BigImageLocation = "/big",
                ThumbnailLocation = "/thumbnail"
            };

            fileStorageConfigOptions.SetupGet(f => f.Value).Returns(fileStorageConfig);

            var options = new DbContextOptionsBuilder<DndContext>()
                .UseInMemoryDatabase("inmemorydbdnd")
                .Options;
            context = new DndContext(options);
            sut = new JournalService(context, _mapper, fileStorageConfigOptions.Object, processor.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            context.Dispose();
        }

        [TestMethod]
        public async Task UploadImageWillSaveAndAddTheImageToTheJournalItem()
        {
            // arrange
            var formFile = new Mock<IFormFile>(MockBehavior.Strict);

            var game = DataCreator.GetGame();
            var addJournalItemDto = new AddJournalItemDto
            {
                JournalItem = new JournalHandoutDto
                {
                    Name = "test"
                }
            };

            await game.AddJournalItemAsync(addJournalItemDto);
            var journalItem = game.JournalItems.First();
            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            formFile.SetupGet(f => f.FileName).Returns("test.png");

            var path = fileStorageConfig.BigImageLocation + game.Id;

            processor.Setup(p => p.SaveImage(formFile.Object, path, It.IsAny<string>())).Returns(Task.CompletedTask).Verifiable();

            // act
            var result = await sut.UploadImage(formFile.Object, game.Id, journalItem.Id);

            // assert
            processor.VerifyAll();

            var journalItemResult = context.JournalItems.Include(j => j.Image).First(j => j.Id == journalItem.Id);
            journalItemResult.Image.Extension.ShouldBe(".png");
            journalItemResult.Image.OriginalName.ShouldBe("test.png");

            result.ShouldNotBe(Guid.Empty);
        }

        [TestMethod]
        public void UploadImageWillThrowBadImageFormatExceptionOnBadExtension()
        {
            // arrange
            var formFile = new Mock<IFormFile>(MockBehavior.Strict);
            var gameId = Guid.NewGuid();
            var extension = ".wtf";

            formFile.SetupGet(f => f.FileName).Returns("test" + extension);

            // act
            var result = Should.Throw<BadImageFormatException>(
                async () => await sut.UploadImage(formFile.Object, gameId, Guid.Empty));

            // assert
            result.Message.ShouldBe($"The format {extension} is not supported");
        }

        [TestMethod]
        public async Task GetJournalItemByIdReturnsTheCorrectJournalItem()
        {
            // arrange
            var game = DataCreator.GetGame();
            var addJournalItemDto = new AddJournalItemDto
            {
                JournalItem = new JournalHandoutDto
                {
                    Name = "test"
                }
            };

            await game.AddJournalItemAsync(addJournalItemDto);
            var journalItem = game.JournalItems.First();
            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();
         
            // act
            var result = await sut.GetJournalItemById(game.Owner.Id, journalItem.Id);

            // assert
            result.Name.ShouldBe(journalItem.Name);
            result.ShouldBeOfType<JournalHandoutDto>();
        }

        [TestMethod]
        public async Task GetJournalItemByIdThrowsAnExceptionIfTheUserDoesNotHavePermissionToSeeJournalItem()
        {
            // arrange
            var game = DataCreator.GetGame();
            var addJournalItemDto = new AddJournalItemDto
            {
                JournalItem = new JournalHandoutDto
                {
                    Name = "test"
                }
            };

            await game.AddJournalItemAsync(addJournalItemDto);
            var journalItem = game.JournalItems.First();
            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var userId = Guid.NewGuid();
          
            // act
            var result = Should.Throw<PermissionException>(async () => await sut.GetJournalItemById(userId, journalItem.Id));

            // assert
            result.Message.ShouldBe("Access Denied");
        }

        //[TestMethod]
        //public async Task AddJournalItemToGameAddsTheJournalItemToTheGame()
        //{
        //    // arrange
        //    var gameId = Guid.NewGuid();
        //    var addJournalItemModel = new AddJournalItemDto
        //    {
        //        JournalItem = new JournalItemDto()
        //    };

        //    var journalItem = new JournalItemMock
        //    {
        //        Id = Guid.Empty,
        //        Name = "test",
        //        ParentFolderId = Guid.Empty,
        //        Type = JournalItemType.Folder,
        //        ImageId = Guid.Empty
        //    };

        //    var mockGame1 = new Mock<Game>();
        //    mockGame1.SetupGet(g => g.Id).Returns(gameId);
        //    mockGame1.Setup(g => g.AddJournalItemAsync(addJournalItemModel)).ReturnsAsync(journalItem);

        //    var gameQueryable = new List<Game> { mockGame1.Object };

        //    var mock = gameQueryable.AsQueryable().BuildMock();

        //    repository.SetupGet(r => r.Games).Returns(mock.Object);
        //    repository.Setup(r => r.Commit()).Returns(Task.CompletedTask).Verifiable();

        //    // act
        //    var (result, _) = await sut.AddJournalItemToGame(addJournalItemModel, gameId);

        //    // assert
        //    result.Name.ShouldBe(journalItem.Name);
        //    result.Id.ShouldBe(journalItem.Id);
        //    result.ImageId.ShouldBe(journalItem.ImageId);
        //    result.ParentFolderId.ShouldBe(journalItem.ParentFolderId);
        //    result.Type.ShouldBe(journalItem.Type);

        //    repository.VerifyAll();
        //}

        //[TestMethod]
        //public async Task AddJournalItemToGameAddsJournalItemToParent()
        //{
        //    // arrange
        //    var gameId = Guid.NewGuid();
        //    var addJournalItemModel = new AddJournalItemDto
        //    {
        //        JournalItem = new JournalItemDto(),
        //        ParentFolderId = Guid.NewGuid()
        //    };

        //    var journalItem = new JournalItemMock
        //    {
        //        Id = Guid.Empty,
        //        Name = "test",
        //        ParentFolderId = Guid.NewGuid(),
        //        Type = JournalItemType.Folder,
        //        ImageId = Guid.Empty
        //    };

        //    // Journalitem mock

        //    var mockJournalItem = new Mock<JournalFolder>();
        //    mockJournalItem.SetupGet(s => s.Id).Returns(addJournalItemModel.ParentFolderId.Value);
        //    mockJournalItem.Setup(s => s.AddJournalItem(addJournalItemModel, gameId)).ReturnsAsync(journalItem);

        //    var journalItemList = new List<JournalFolder> { mockJournalItem.Object };
        //    var journalFolderQueryable = journalItemList.AsQueryable().BuildMock();

        //    repository.SetupGet(r => r.JournalFolders).Returns(journalFolderQueryable.Object);

        //    // Others
        //    repository.Setup(r => r.Commit()).Returns(Task.CompletedTask).Verifiable();

        //    // act
        //    var (result, _) = await sut.AddJournalItemToGame(addJournalItemModel, gameId);

        //    // assert
        //    result.Name.ShouldBe(journalItem.Name);
        //    result.Id.ShouldBe(journalItem.Id);
        //    result.ImageId.ShouldBe(journalItem.ImageId);
        //    result.ParentFolderId.ShouldBe(journalItem.ParentFolderId);
        //    result.Type.ShouldBe(journalItem.Type);

        //    repository.VerifyAll();
        //}


        //[TestMethod]
        //public async Task GetJournalItemsForParentFolderIdForOwner()
        //{
        //    // arrange
        //    var gameId = Guid.NewGuid();
        //    var userId = Guid.NewGuid();
        //    var parentFolderId = Guid.NewGuid();

        //    var journalItem = new JournalFolder
        //    {
        //        Id = Guid.Empty,
        //        Name = "test",
        //        ParentFolderId = parentFolderId,
        //        Type = JournalItemType.Folder,
        //        ImageId = Guid.Empty
        //    };

        //    var journalItemList = new List<JournalFolder> { journalItem };
        //    var journalFolderQueryable = journalItemList.AsQueryable().BuildMock();

        //    repository.SetupGet(r => r.JournalItems).Returns(journalFolderQueryable.Object);

        //    // Game mock
        //    var mockGame1 = new Mock<Game>();
        //    mockGame1.SetupGet(g => g.Id).Returns(gameId);
        //    mockGame1.SetupGet(g => g.Owner.Id).Returns(userId);

        //    var gameQueryable = new List<Game> { mockGame1.Object };

        //    var mock = gameQueryable.AsQueryable().BuildMock();

        //    repository.SetupGet(r => r.Games).Returns(mock.Object);

        //    // act
        //    var result = await sut.GetJournalItemsForParentFolderId(userId, gameId, parentFolderId);

        //    // assert
        //    var resultObject = result.First();
        //    result.Count().ShouldBe(1);
        //    resultObject.Name.ShouldBe(journalItem.Name);
        //    resultObject.Id.ShouldBe(journalItem.Id);
        //    resultObject.ImageId.ShouldBe(journalItem.ImageId);
        //    resultObject.ParentFolderId.ShouldBe(journalItem.ParentFolderId);
        //    resultObject.Type.ShouldBe(journalItem.Type);
        //}

        //[TestMethod]
        //public async Task GetJournalItemsForNullParentFolderId()
        //{
        //    // arrange
        //    var gameId = Guid.NewGuid();
        //    var userId = Guid.NewGuid();

        //    var journalItem = new JournalFolder
        //    {
        //        Id = Guid.Empty,
        //        Name = "test",
        //        Type = JournalItemType.Folder,
        //        ImageId = Guid.Empty,
        //        GameId = gameId
        //    };

        //    var journalItem2 = new JournalFolder
        //    {
        //        Id = Guid.Empty,
        //        Name = "test2",
        //        Type = JournalItemType.Folder,
        //        ImageId = Guid.Empty,
        //        GameId = Guid.Empty
        //    };

        //    var journalItemList = new List<JournalFolder> { journalItem, journalItem2 };
        //    var journalFolderQueryable = journalItemList.AsQueryable().BuildMock();

        //    repository.SetupGet(r => r.JournalItems).Returns(journalFolderQueryable.Object);

        //    // Game mock
        //    var mockGame1 = new Mock<Game>();
        //    mockGame1.SetupGet(g => g.Id).Returns(gameId);
        //    mockGame1.SetupGet(g => g.Owner.Id).Returns(userId);

        //    var gameQueryable = new List<Game> { mockGame1.Object };

        //    var mock = gameQueryable.AsQueryable().BuildMock();

        //    repository.SetupGet(r => r.Games).Returns(mock.Object);

        //    // act
        //    var result = await sut.GetJournalItemsForParentFolderId(userId, gameId, null);

        //    // assert
        //    var resultObject = result.First();
        //    result.Count().ShouldBe(1);
        //    resultObject.Name.ShouldBe(journalItem.Name);
        //    resultObject.Id.ShouldBe(journalItem.Id);
        //    resultObject.ImageId.ShouldBe(journalItem.ImageId);
        //    resultObject.ParentFolderId.ShouldBe(journalItem.ParentFolderId);
        //    resultObject.Type.ShouldBe(journalItem.Type);
        //}

        [TestMethod]
        public async Task GetJournalItemsForParentFolderIdReturnsOnlyFilledFoldersAndNotFolders()
        {
            // arrange
            //var gameId = Guid.NewGuid();
            //var userId = Guid.NewGuid();

            //var permission = new JournalItemPermission
            //{
            //    UserId = userId,
            //    GameId = gameId,
            //    CanEdit = true,
            //    CanSee = true
            //};

            //var handout = new JournalHandout
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "test2",
            //    Type = JournalItemType.Folder,
            //    ImageId = Guid.Empty,
            //    GameId = Guid.Empty,
            //    Permissions = new List<JournalItemPermission> { permission }
            //};

            //var game = new Game
            //{
            //    Id = gameId,
            //    JournalItems = new List<JournalItem> { handout },
            //    Owner = new User()
            //};

            //var journalItemList = new List<JournalItem> { handout };
            //var journalItemQueryable = journalItemList.AsQueryable().BuildMock();

            //var gameQueryable = new List<Game> { game }.AsQueryable().BuildMock();

            //repository.SetupGet(r => r.JournalItems).Returns(journalItemQueryable.Object);
            //repository.SetupGet(r => r.Games).Returns(gameQueryable.Object);

            //// act
            //var result = await sut.GetJournalItemsForParentFolderId(userId, gameId, null);

            //// Assert
            //result.Count().ShouldBe(1);
            //result.First().Id.ShouldBe(handout.Id);
            //result.First().ImageId.ShouldBe(handout.ImageId);
            //result.First().Name.ShouldBe(handout.Name);
            //result.First().ParentFolderId.ShouldBe(handout.ParentFolderId);
        }
    }
}