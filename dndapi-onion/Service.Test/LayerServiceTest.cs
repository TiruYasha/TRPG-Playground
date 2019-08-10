using Domain.Domain.PlayArea;
using Domain.Dto.Shared;
using Domain.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
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
    public class LayerServiceTest : ServiceTest<ILayerService>
    {
        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            Sut = new LayerService(Context, Mapper);
        }

        [TestMethod]
        public async Task AddTokenToLayer_AddsTheTokenToTheLayerAndSaveItInTheDb()
        {
            // Arrange
            var game = await GameDataBuilder
                .WithMaps(true)
                .WithJournalHandout()
                .BuildGame();
            await Context.AddAsync(game);
            await Context.SaveChangesAsync();

            var tokenDto = new CharacterTokenDto
            {
                Y = 10,
                X = 20,
                Type = TokenType.Character,
                CharacterSheetId = Guid.Empty
            };

            // Act
            var result = await Sut.AddTokenToLayer(tokenDto, game.Id, GameDataBuilder.Owner.Id, GameDataBuilder.Layer2.Id);

            // Assert
            var tokens = Context.Layers.Include(l => l.Tokens).First(l => l.Id == GameDataBuilder.Layer2.Id).Tokens;
            tokens.Count.ShouldBe(1);
            result.Id.ShouldBe(tokens.First().Id);
        }
    }
}
