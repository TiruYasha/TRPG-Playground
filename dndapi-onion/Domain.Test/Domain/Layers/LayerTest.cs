using System;
using System.Threading.Tasks;
using Domain.Domain.Layers;
using Domain.Domain.PlayArea;
using Domain.Dto.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Domain.Test.Domain.Layers
{
    [TestClass]
    public class LayerTest
    {
        [TestMethod]
        public void NewLayerSetsNameAndId()
        {
            // Arrange
            var mapId = Guid.NewGuid();

            var dto = new LayerDto
            {
                Name = "test",
                Order = 2
            };

            // Act
            var result = new Layer(dto, mapId);

            // Assert
            result.Name.ShouldBe(dto.Name);
            result.Order.ShouldBe(dto.Order);
            result.Id.ShouldNotBeNull();
            result.Type.ShouldBe(LayerType.Default);
            result.IsVisibleToPlayers.ShouldBe(false);
            result.MapId.ShouldBe(mapId);
        }

        [TestMethod]
        public void NewLayerShouldThrowArgumentExceptionOnEmptyName()
        {
            // Arrange
            var expectedErrorMessage = "Name may not be empty";

            var dto = new LayerDto
            {
                Name = "",
                Order = 2
            };

            // Act
            var result = Should.Throw<ArgumentException>(() => new Layer(dto, Guid.Empty));

            // Assert
            result.Message.ShouldBe(expectedErrorMessage);
        }

        [TestMethod]
        public async Task UpdateUpdatesTheLayer()
        {
            // Arrange
            var dto = new LayerDto
            {
                Name = "test",
                Order = 2
            };
            var sut = new Layer(dto, new Guid());

            var updatedName = "updated";

            // Act
            await sut.Update(updatedName);

            // Assert
            sut.Name.ShouldBe(updatedName);
        }

        [TestMethod]
        public async Task UpdateShouldThrowArgumentExceptionOnEmptyName()
        {
            // Arrange
            var expectedErrorMessage = "Name may not be empty";
            var dto = new LayerDto
            {
                Name = "test",
                Order = 2
            };
            var sut = new Layer(dto, new Guid());

            // Act
            var result = await Should.ThrowAsync<ArgumentException>(sut.Update(""));

            // Assert
            result.Message.ShouldBe(expectedErrorMessage);
        }

        [TestMethod]
        public async Task UpdateOrderUpdatesTheOrder()
        {
            // Arrange
            var newOrder = 1;
            var dto = new LayerDto
            {
                Name = "test",
                Order = 2
            };
            var sut = new Layer(dto, new Guid());

            // Act
            await sut.UpdateOrder(newOrder);

            // Assert
            sut.Order.ShouldBe(newOrder);
        }

        [TestMethod]
        public async Task AddToken_AddsTheTokenToTheLayer()
        {
            // Arrange
            var dto = new LayerDto
            {
                Name = "test",
            };
            var sut = new Layer(dto, new Guid());

            var tokenDto = new TokenDto
            {
                Y = 10,
                X = 20,
                Type = TokenType.Default
            };

            // Act
            var token = await sut.AddToken(tokenDto);

            //Assert
            sut.Tokens.ShouldContain(token);
        }

        [TestMethod]
        public async Task ToggleVisibleToPlayers_SetThePropertyIsVisibleToPlayersToTrue()
        {
            // Arrange
            var dto = new LayerDto
            {
                Name = "test",
            };
            var sut = new Layer(dto, new Guid());

            // Act
            await sut.ToggleVisibleToPlayers();

            //Assert
            sut.IsVisibleToPlayers.ShouldBe(true);
        }

        [TestMethod]
        public async Task ToggleVisibleToPlayers_SetThePropertyIsVisibleToPlayersToFalseIfItIsTrue()
        {
            // Arrange
            var dto = new LayerDto
            {
                Name = "test",
            };
            var sut = new Layer(dto, new Guid());
            await sut.ToggleVisibleToPlayers();

            // Act
            await sut.ToggleVisibleToPlayers();

            //Assert
            sut.IsVisibleToPlayers.ShouldBe(false);
        }
    }
}
