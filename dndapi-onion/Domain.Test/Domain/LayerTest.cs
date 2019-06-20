using System;
using Domain.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Domain.Test.Domain
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
    }
}
