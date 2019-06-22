using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Dto.RequestDto;
using Domain.MappingProfiles;
using Domain.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;

namespace Service.Test
{
    [TestClass]
    public class PlayAreaServiceTest : ServiceTest<IPlayAreaService>
    {
        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            sut = new PlayAreaService(context, mapper);
        }

        [TestMethod]
        public async Task AddMapAddsTheMapToThePlayArea()
        {
            // Arrange
            var game = await gameDataBuilder.BuildGame();
            await context.AddAsync(game);
            await context.SaveChangesAsync();

            var addMapDto = new AddMapDto
            {
                GridSizeInPixels = 40,
                HeightInPixels = 400,
                Name = "test",
                WidthInPixels = 400
            };

            // Act
            var result = await sut.AddMap(addMapDto, game.PlayArea.Id, game.Id);

            // Assert
            var map = await context.Maps.FirstAsync();
            result.Id.ShouldBe(map.Id);
            result.GridSizeInPixels.ShouldBe(addMapDto.GridSizeInPixels);
            result.HeightInPixels.ShouldBe(addMapDto.HeightInPixels);
            result.Name.ShouldBe(addMapDto.Name);
            result.WidthInPixels.ShouldBe(addMapDto.WidthInPixels);
        }
    }
}
