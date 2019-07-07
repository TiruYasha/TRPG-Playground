﻿using Domain.Dto.Shared;
using Domain.Exceptions;
using Domain.ServiceInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Domain.Layers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
        public async Task AddLayerAddsTheLayerToTheLayerGroup()
        {
            // arrange
            var game = await GameDataBuilder
                .WithMaps(true)
                .BuildGame();
            await Context.AddAsync(game);
            await Context.SaveChangesAsync();

            var map = game.Maps.Last();
            var layerGroup = map.Layers.Last();
            var layerToAdd = new LayerDto
            {
                MapId = map.Id,
                Name = "testGroup",
                Type = LayerType.Default,
                LayerGroupId = layerGroup.Id
            };

            // Act
            await Sut.AddLayer(layerToAdd, map.Id, game.Id);

            // Assert
            var result = await Context.LayerGroups.Include(l => l.Layers).FirstOrDefaultAsync(l => l.Id == layerGroup.Id);
            result.Layers.Count.ShouldBe(1);
            result.Layers.First().Name.ShouldBe(layerToAdd.Name);
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
            var layerToUpdate = map.Layers.First();

            var updateValues = new LayerDto
            {
                Id = layerToUpdate.Id,
                Name = "updated",
            };

            // Act
            await Sut.UpdateLayer(updateValues, map.Id, game.Id);

            // Assert
            var layer = await Context.Layers.FirstOrDefaultAsync(l => l.Id == updateValues.Id);
            layer.Name.ShouldBe(updateValues.Name);
        }

        [TestMethod]
        public async Task DeleteLayerDeletesTheLayer()
        {
            // arrange
            var game = await GameDataBuilder
                .WithMaps()
                .BuildGame();
            await Context.AddAsync(game);
            await Context.SaveChangesAsync();

            var map = game.Maps.First();
            var layerToDelete = map.Layers.First();

            // Act
            await Sut.DeleteLayer(layerToDelete.Id, map.Id, game.Id);

            // Assert
            var layer = await Context.Layers.FirstOrDefaultAsync(l => l.Id == layerToDelete.Id);
            layer.ShouldBeNull();
        }

        [TestMethod]
        public async Task GetLayersGetsAllTheLayers()
        {
            // arrange
            var game = await GameDataBuilder
                .WithMaps(true)
                .BuildGame();
            await Context.AddAsync(game);
            await Context.SaveChangesAsync();

            var map = game.Maps.First();
            var layer1 = map.Layers.First();

            // Act
            var result = await Sut.GetLayers(map.Id, game.Id);

            // Assert
            result.Count().ShouldBe(2);
            var layer = result.First();
            layer.Id.ShouldBe(layer1.Id);
            layer.Name.ShouldBe(layer1.Name);
            layer.Type.ShouldBe(layer1.Type);
            layer.MapId.ShouldBe(layer1.MapId);
        }

        //[TestMethod]
        //public async Task UpdateLayerOrder_OnHigherOrder_LowerTheOthers()
        //{
        //    // Arrange
        //    var game = await GameDataBuilder
        //        .WithMaps(true)
        //        .BuildGame();
        //    await Context.AddAsync(game);
        //    await Context.SaveChangesAsync();

        //    var map = game.Maps.First();
        //    var layerToUpdate = map.Layers.Last();

        //    var dto = new ChangeOrderDto
        //    {
        //        NewPosition = 0,
        //        PreviousPosition = layerToUpdate.Order
        //    };

        //    //Act
        //    await Sut.UpdateLayerOrder(dto, layerToUpdate.Id, map.Id, game.Id);

        //    // Assert
        //    var layerToUpdateUpdated = await Context.Layers.FindAsync(layerToUpdate.Id);
        //    var secondLayer = await Context.Layers.Where(o => o.Order == 1).FirstOrDefaultAsync();

        //    layerToUpdateUpdated.Order.ShouldBe(0);
        //    secondLayer.ShouldNotBeNull();
        //    secondLayer.Id.ShouldNotBe(layerToUpdate.Id);
        //}
    }
}
