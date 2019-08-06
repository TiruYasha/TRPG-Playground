using Domain.ServiceInterfaces;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Domain.Dto.Shared;

namespace Service.Test
{
    [TestClass]
    public class GameServiceTest : ServiceTest<IGameService>
    {
        private Mock<ILogger<GameService>> logger;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            logger = new Mock<ILogger<GameService>>();

            Sut = new GameService(Context, Mapper, logger.Object);
        }

        [TestMethod]
        public async Task AddMapAddsTheMapToThePlayArea()
        {
            // Arrange
            var game = await GameDataBuilder.BuildGame();
            await Context.AddAsync(game);
            await Context.SaveChangesAsync();

            var addMapDto = new MapDto
            {
                GridSizeInPixels = 40,
                HeightInPixels = 400,
                Name = "test",
                WidthInPixels = 400
            };

            // Act
            var result = await Sut.AddMap(addMapDto, game.Id);

            // Assert
            var map = await Context.Maps.FirstAsync();
            result.Id.ShouldBe(map.Id);
            result.GridSizeInPixels.ShouldBe(addMapDto.GridSizeInPixels);
            result.HeightInPixels.ShouldBe(addMapDto.HeightInPixels);
            result.Name.ShouldBe(addMapDto.Name);
            result.WidthInPixels.ShouldBe(addMapDto.WidthInPixels);
        }

        [TestMethod]
        public async Task AddMapThrowNotFoundExceptionWhenGameOrPlayAreaCantBeFound()
        {
            // Arrange
            var MapDto = new MapDto();

            // Act
            var result = await Should.ThrowAsync<NotFoundException>(Sut.AddMap(MapDto, new Guid()));

            // Assert
            result.Message.ShouldBe("The game can not be found");
        }

        [TestMethod]
        public async Task GetMapsReturnsMaps()
        {
            // Arrange
            var game = await GameDataBuilder.WithMaps().BuildGame();
            await Context.AddAsync(game);
            await Context.SaveChangesAsync();

            // Act
            var result = await Sut.GetMaps(game.Id);

            // Assert
            result.Count().ShouldBe(2);
        }

        [TestMethod]
        public async Task SetMapVisible_SetsTheMapVisibleByMapId()
        {
            // Arrange
            var game = await GameDataBuilder.WithMaps().BuildGame();
            await Context.AddAsync(game);
            await Context.SaveChangesAsync();

            var map = game.Maps.First();

            // Act
            var result = await Sut.SetMapVisible(game.Id, map.Id);

            // Assert
            var updatedGame = await Context.Games.FirstAsync(g  => g.Id == game.Id);
            result.Id.ShouldBe(map.Id);
        }

        [TestMethod]
        public async Task SetMapVisible_ThrowsNotFoundExceptionIfMapDoesNotExist()
        {
            // Arrange
            var game = await GameDataBuilder.WithMaps().BuildGame();
            await Context.AddAsync(game);
            await Context.SaveChangesAsync();

            // Act
            var result = await Should.ThrowAsync<NotFoundException>(Sut.SetMapVisible(game.Id, Guid.NewGuid()));

            // Assert
            result.Message.ShouldBe("The game or map can not be found");
        }

        [TestMethod]
        public async Task SetMapVisible_ThrowsNotFoundExceptionIfGameDoesNotExist()
        {
            // Act
            var result = await Should.ThrowAsync<NotFoundException>(Sut.SetMapVisible(Guid.NewGuid(), Guid.NewGuid()));

            // Assert
            result.Message.ShouldBe("The game or map can not be found");
        }
    }
}
