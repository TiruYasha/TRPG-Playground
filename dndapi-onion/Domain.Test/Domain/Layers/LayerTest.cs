using System;
using System.Threading.Tasks;
using Domain.Domain.Layers;
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
            var name = "test";

            // Act
            var result = new Layer(name);

            // Assert
            result.Name.ShouldBe(name);
            result.Id.ShouldNotBeNull();
            result.Type.ShouldBe(LayerType.Default);
        }

        [TestMethod]
        public void NewLayerShouldThrowArgumentExceptionOnEmptyName()
        {
            // Arrange
            var expectedErrorMessage = "Name may not be empty";

            // Act
            var result = Should.Throw<ArgumentException>(() => new Layer(""));

            // Assert
            result.Message.ShouldBe(expectedErrorMessage);
        }

        [TestMethod]
        public async Task UpdateUpdatesTheLayer()
        {
            // Arrange
            var name = "test";
            var sut = new Layer(name);

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

            var name = "test";
            var sut = new Layer(name);

            // Act
            var result = await Should.ThrowAsync<ArgumentException>(sut.Update(""));

            // Assert
            result.Message.ShouldBe(expectedErrorMessage);
        }
    }
}
