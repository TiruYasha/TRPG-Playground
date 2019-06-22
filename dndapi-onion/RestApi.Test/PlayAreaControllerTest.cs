using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.Dto.RequestDto;
using Domain.Dto.Shared;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestApi.Utilities;
using Shouldly;

namespace RestApi.Test
{
    [TestClass]
    public class PlayAreaControllerTest
    {
        private PlayAreaController sut;

        private Mock<IPlayAreaService> playAreaService;
        private Mock<IJwtReader> jwtReader;

        [TestInitialize]
        public void Initialize()
        {
            playAreaService = new Mock<IPlayAreaService>(MockBehavior.Strict);
            jwtReader = new Mock<IJwtReader>(MockBehavior.Strict);

            sut = new PlayAreaController(playAreaService.Object, jwtReader.Object);
        }

        [TestMethod]
        public async Task AddMapToPlayAreaReturnsOkWithMap()
        {
            // Arrange
            var addMapDto = new AddMapDto();
            var mapToReturn = new MapDto();
            var playAreaId = Guid.NewGuid();
            var gameId = Guid.NewGuid();

            jwtReader.Setup(j => j.GetGameId()).Returns(gameId);

            playAreaService.Setup(p => p.AddMap(addMapDto, playAreaId, gameId)).ReturnsAsync(mapToReturn);

            // Act
            var result = await sut.AddMapToPlayArea(playAreaId, addMapDto) as OkObjectResult;

            // Assert
            result.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            result.Value.ShouldBe(mapToReturn);
        }
    }
}
