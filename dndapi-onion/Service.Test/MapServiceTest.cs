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
using Domain.Domain.Layers;
using Microsoft.EntityFrameworkCore;

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

        [TestMethod]
        public async Task AddLayerAddsTheLayer()
        {
            // arrange
            var game = await GameDataBuilder
                .WithMaps()
                .BuildGame();
            await Context.AddAsync(game);
            await Context.SaveChangesAsync();

            var map = game.Maps.First();

            var layerToAdd = new LayerDto
            {
                MapId = map.Id,
                Name = "testing",
                Type = LayerType.Default
            };

            // Act
            var result = await Sut.AddLayer(layerToAdd, map.Id, game.Id);

            // Assert
            var layer = await Context.Maps.Include(m => m.Layers).SelectMany(m => m.Layers).FirstOrDefaultAsync(l => l.Id == result.Id);
            result.Id.ShouldBe(layer.Id);
            layer.Name.ShouldBe(layerToAdd.Name);
            layer.Type.ShouldBe(layerToAdd.Type);
        }

        [TestMethod]
        public async Task AddLayerAddsTheLayerGroup()
        {
            // arrange
            var game = await GameDataBuilder
                .WithMaps()
                .BuildGame();
            await Context.AddAsync(game);
            await Context.SaveChangesAsync();

            var map = game.Maps.First();

            var layerToAdd = new LayerDto
            {
                MapId = map.Id,
                Name = "testGroup",
                Type = LayerType.Group
            };

            // Act
            var result = await Sut.AddLayer(layerToAdd, map.Id, game.Id);

            // Assert
            var layer = await Context.Maps.Include(m => m.Layers).SelectMany(m => m.Layers).FirstOrDefaultAsync(l => l.Id == result.Id);
            result.Id.ShouldBe(layer.Id);
            layer.Name.ShouldBe(layerToAdd.Name);
            layer.Type.ShouldBe(layerToAdd.Type);
        }

        [TestMethod]
        public async Task UpdateLayerUpdatesTheLayer()
        {
            // arrange
            var game = await GameDataBuilder
                .WithMaps()
                .BuildGame();
            await Context.AddAsync(game);
            await Context.SaveChangesAsync();

            var map = game.Maps.First();

            var layerToAdd = new LayerDto
            {
                MapId = map.Id,
                Name = "testing",
                Type = LayerType.Default
            };

            // Act
            var result = await Sut.AddLayer(layerToAdd, map.Id, game.Id);

            // Assert
            var layer = await Context.Maps.Include(m => m.Layers).SelectMany(m => m.Layers).FirstOrDefaultAsync(l => l.Id == result.Id);
            result.Id.ShouldBe(layer.Id);
            layer.Name.ShouldBe(layerToAdd.Name);
            layer.Type.ShouldBe(layerToAdd.Type);
        }
    }
}
