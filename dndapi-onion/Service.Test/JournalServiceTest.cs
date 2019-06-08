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

        private Mock<IOptions<FileStorageConfig>> fileStorageConfigOptions;
        private Mock<ImageProcesser> processor;

        private DndContext context;

        private FileStorageConfig fileStorageConfig;
        private IJournalService sut;

        private GameDataBuilder gameDataBuilder;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            IConfigurationProvider config = new MapperConfiguration(d => d.AddProfile<MyProfile>());
            _mapper = new Mapper(config);
        }

        [TestInitialize]
        public void Initialize()
        {

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

            gameDataBuilder = new GameDataBuilder();
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

            var game = await gameDataBuilder.BuildGame();
            var addJournalItemDto = new AddJournalItemDto
            {
                JournalItem = new JournalHandoutDto
                {
                    Name = "test"
                }
            };

            await game.AddJournalItem(addJournalItemDto);
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
            var game = await gameDataBuilder.BuildGame();
            var addJournalItemDto = new AddJournalItemDto
            {
                JournalItem = new JournalHandoutDto
                {
                    Name = "test"
                }
            };

            await game.AddJournalItem(addJournalItemDto);
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
            var game = await gameDataBuilder.BuildGame();
            var addJournalItemDto = new AddJournalItemDto
            {
                JournalItem = new JournalHandoutDto
                {
                    Name = "test"
                }
            };

            await game.AddJournalItem(addJournalItemDto);
            var journalItem = game.JournalItems.First();
            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var userId = Guid.NewGuid();

            // act
            var result = Should.Throw<PermissionException>(async () => await sut.GetJournalItemById(userId, journalItem.Id));

            // assert
            result.Message.ShouldBe("Access Denied");
        }

        [TestMethod]
        public async Task UpdateJournalItemUpdatesTheJournalItem()
        {
            // arrange
            var game = await gameDataBuilder.WithJournalHandout(true).BuildGame();
            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var journalItemId = game.JournalItems.First().Id;

            var journalItemDto = new JournalHandoutDto
            {
                Id = journalItemId,
                Name = "Updated",
                Description = "This is also updated",
                CanEdit = new List<Guid> { gameDataBuilder.Player1.Id },
                CanSee = new List<Guid> { gameDataBuilder.Player1.Id},
                OwnerNotes = "OwnerNotesUpdated",
                ImageId = Guid.NewGuid()
            };

            // act
            var result = await sut.UpdateJournalItem(journalItemDto, game.Id, game.Owner.Id);

            // assert
            result.Name.ShouldBe(journalItemDto.Name);
            result.Type.ShouldBe(JournalItemType.Handout);
            result.Id.ShouldBe(journalItemId);

            var journalItemInDb = await context.JournalItems.Include(j => j.Permissions).FirstAsync(j => j.Id == journalItemId) as JournalHandout;
            journalItemInDb.Name.ShouldBe(journalItemDto.Name);
            journalItemInDb.Description.ShouldBe(journalItemDto.Description);
            journalItemInDb.OwnerNotes.ShouldBe(journalItemDto.OwnerNotes);
            journalItemInDb.Permissions.Count.ShouldBe(1);
        }

        [TestMethod]
        public async Task UpdateJournalItemThrowsExceptionIfPlayerDoesNotHaveEditPermission()
        {
            // arrange
            var game = await gameDataBuilder.WithJournalHandout(true).BuildGame();
            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var journalItemId = game.JournalItems.First().Id;

            var journalItemDto = new JournalHandoutDto
            {
                Id = journalItemId,
                Name = "Updated",
                Description = "This is also updated",
                CanEdit = new List<Guid> { gameDataBuilder.Player1.Id },
                CanSee = new List<Guid> { gameDataBuilder.Player1.Id },
                OwnerNotes = "OwnerNotesUpdated",
                ImageId = Guid.NewGuid()
            };

            // act
            var result =  Should.Throw<PermissionException>(async () => await sut.UpdateJournalItem(journalItemDto, game.Id, gameDataBuilder.Player1.Id));

            // assert
            result.Message.ShouldBe("You do not have permission or the journalitem could not be found");
        }

        [TestMethod]
        public async Task GetJournalItemPermissionsReturnJournalItemPermissions()
        {
            // arrange
            var game = await gameDataBuilder.WithJournalHandout(true).BuildGame();
            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            var journalItemId = game.JournalItems.First().Id;
         
            // act
            var result = await sut.GetJournalItemPermissions(journalItemId);

            // assert
            result.Count().ShouldBe(2);
            result.Any(p => p.UserId == gameDataBuilder.Player1.Id).ShouldBeTrue();
            result.Any(p => p.UserId == gameDataBuilder.Player2.Id).ShouldBeTrue();
        }
    }
}