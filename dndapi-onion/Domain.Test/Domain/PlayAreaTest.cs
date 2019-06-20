using System;
using System.Threading.Tasks;
using Domain.Domain;
using Domain.Dto.RequestDto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Domain.Test.Domain
{
    [TestClass]
    public class PlayAreaTest
    {

        [TestMethod]
        public void NewPlayAreaInitializesVariables()
        {
            // Arrange
            var gameId = new Guid();

            // act
            var result = new PlayArea(gameId);

            // assert
            result.Id.ShouldNotBeNull();
            result.Maps.ShouldNotBeNull();
            result.Maps.Count.ShouldBe(0);
            result.GameId.ShouldBe(gameId);
        }

        [TestMethod]
        public async Task AddMapAddsTheMapToTheList()
        {
            // arrange
            var dto = new AddMapDto
            {
                Name = "test",
                GridSizeInPixels = 10,
                WidthInPixels = 1000,
                HeightInPixels = 1000
            };

            var sut = new PlayArea(new Guid());

            // Act
            var result = await sut.AddMap(dto);

            // Assert
            result.Name.ShouldBe(dto.Name);
            sut.Maps.Count.ShouldBe(1);
            sut.Maps.ShouldContain(result);
        }
    }
}
