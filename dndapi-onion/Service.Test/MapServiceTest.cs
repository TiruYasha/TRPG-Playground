using Domain.Dto.RequestDto;
using Domain.Dto.Shared;
using Domain.Exceptions;
using Domain.ServiceInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Test
{
    [TestClass]
    public class MapServiceTest : ServiceTest<IMapService>
    {
        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            Sut = new MapService(Context, Mapper);
        }

        [TestMethod]
        public async Task UpdateMapUpdatesTheMap()
        {
            // Arrange
            var game = await GameDataBuilder
                .WithMaps()
                .BuildGame();
            await Context.AddAsync(game);
            await Context.SaveChangesAsync();
            
            var mapToUpdate = game.Maps.First();

            var updateValues = new MapDto
            {
                Id = mapToUpdate.Id,
                GridSizeInPixels = 50,
                HeightInPixels = 500,
                Name = "updated",
                WidthInPixels = 500
            };

            // act
            await Sut.UpdateMap(updateValues, game.Id);

            // assert
            var map = Context.Maps.First(f => f.Id == mapToUpdate.Id);
            map.GridSizeInPixels.ShouldBe(updateValues.GridSizeInPixels);
            map.HeightInPixels.ShouldBe(updateValues.HeightInPixels);
            map.Name.ShouldBe(updateValues.Name);
            map.WidthInPixels.ShouldBe(updateValues.WidthInPixels);
        }

        [TestMethod]
        public async Task UpdateMapThrowsNotFoundException()
        {
            // Arrange
            var game = await GameDataBuilder
                .BuildGame();
            await Context.AddAsync(game);
            await Context.SaveChangesAsync();

            var updateValues = new MapDto
            {
                Id = new Guid(),
                GridSizeInPixels = 50,
                HeightInPixels = 500,
                Name = "updated",
                WidthInPixels = 500
            };

            // act
            var result = await Should.ThrowAsync<NotFoundException>(Sut.UpdateMap(updateValues, game.Id));

            // assert
            result.Message.ShouldBe("The map could not be found");
        }

        [TestMethod]
        public async Task DeleteMapDeletesTheMap()
        {
            // Arrange
            var game = await GameDataBuilder
               .WithMaps()
               .BuildGame();
            await Context.AddAsync(game);
            await Context.SaveChangesAsync();

            var mapToDelete = game.Maps.First();

            //Act
            await Sut.DeleteMap(mapToDelete.Id, game.Id);

            // Assert
            var map = Context.Maps.FirstOrDefault(f => f.Id == mapToDelete.Id);
            map.ShouldBeNull();
        }
    }
}
