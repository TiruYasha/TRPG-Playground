using System;
using System.Linq;
using Domain.Domain;
using Domain.Dto.RequestDto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Domain.Test.Domain
{
    [TestClass]
    public class MapTest
    {
        [TestMethod]
        public void NewMapSetsPropertiesFromDto()
        {
            // Arrange
            var dto = new AddMapDto
            {
                Name = "test",
                GridSizeInPixels = 10,
                WidthInPixels = 1000,
                HeightInPixels = 1000
            };

            // Act
            var result = new Map(dto);

            // Assert
            result.Id.ShouldNotBeNull();
            result.Name.ShouldBe(dto.Name);
            result.WidthInPixels.ShouldBe(dto.WidthInPixels);
            result.HeightInPixels.ShouldBe(dto.HeightInPixels);
            result.GridSizeInPixels.ShouldBe(dto.GridSizeInPixels);
        }

        [TestMethod]
        public void NewMapCreatesStandardLayer()
        {
            // Arrange
            var dto = new AddMapDto
            {
                Name = "test",
                GridSizeInPixels = 10,
                WidthInPixels = 1000,
                HeightInPixels = 1000
            };

            // Act
            var result = new Map(dto);

            // Assert
            result.Layers.Count.ShouldBe(1);
            var layer = result.Layers.First();
            layer.Name.ShouldBe("layer1");
        }

        [TestMethod]
        public void NewMapEmptyNameIsNotAllowed()
        {
            // Arrange
            var dto = new AddMapDto
            {
                Name = "",
                GridSizeInPixels = 10,
                WidthInPixels = 1000,
                HeightInPixels = 1000
            };

            var expectedErrorMessage = "Name may not be empty";

            // Act
            var result = Should.Throw<ArgumentException>(() => new Map(dto));

            // Assert
            result.Message.ShouldBe(expectedErrorMessage);
        }

        [TestMethod]
        public void NewMapWidthInPixelsMayNotBeHigherThan4000()
        {
            // Arrange
            var dto = new AddMapDto
            {
                Name = "test",
                GridSizeInPixels = 10,
                WidthInPixels = 4001,
                HeightInPixels = 1000
            };

            var expectedErrorMessage = "Width may not be larger than 4000 pixels";

            // Act
            var result = Should.Throw<ArgumentException>(() => new Map(dto));

            // Assert
            result.Message.ShouldBe(expectedErrorMessage);
        }

        [TestMethod]
        public void NewMapWidthInPixelsMayNotBeLowerThan0()
        {
            // Arrange
            var dto = new AddMapDto
            {
                Name = "test",
                GridSizeInPixels = 10,
                WidthInPixels = -1,
                HeightInPixels = 1000
            };

            var expectedErrorMessage = "Width may not be smaller than 0 pixels";

            // Act
            var result = Should.Throw<ArgumentException>(() => new Map(dto));

            // Assert
            result.Message.ShouldBe(expectedErrorMessage);
        }

        [TestMethod]
        public void NewMapHeightInPixelsMayNotBeHigherThan4000()
        {
            // Arrange
            var dto = new AddMapDto
            {
                Name = "test",
                GridSizeInPixels = 10,
                WidthInPixels = 1000,
                HeightInPixels = 4001
            };

            var expectedErrorMessage = "Height may not be larger than 4000 pixels";

            // Act
            var result = Should.Throw<ArgumentException>(() => new Map(dto));

            // Assert
            result.Message.ShouldBe(expectedErrorMessage);
        }

        [TestMethod]
        public void NewMapHeightInPixelsMayNotBeLowerThan0()
        {
            // Arrange
            var dto = new AddMapDto
            {
                Name = "test",
                GridSizeInPixels = 10,
                WidthInPixels = 1000,
                HeightInPixels = -1
            };

            var expectedErrorMessage = "Height may not be smaller than 0 pixels";

            // Act
            var result = Should.Throw<ArgumentException>(() => new Map(dto));

            // Assert
            result.Message.ShouldBe(expectedErrorMessage);
        }

        [TestMethod]
        public void NewMapGridSizeInPixelsMayNotBeLowerThan0()
        {
            // Arrange
            var dto = new AddMapDto
            {
                Name = "test",
                GridSizeInPixels = -1,
                WidthInPixels = 1000,
                HeightInPixels = 100
            };

            var expectedErrorMessage = "Grid size may not be smaller than 0 pixels";

            // Act
            var result = Should.Throw<ArgumentException>(() => new Map(dto));

            // Assert
            result.Message.ShouldBe(expectedErrorMessage);
        }
    }
}
