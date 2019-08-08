using Domain.Domain.PlayArea;
using Domain.Dto.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

namespace Domain.Test.Domain.PlayArea
{
    [TestClass]
    public class CharacterTokenTest
    {
        [TestMethod]
        public void Constructor_InitializesAllValues()
        {
            // Arrange
            var characterTokenDto = new CharacterTokenDto()
            {
                Y = 10,
                X = 20,
                Type = TokenType.Default,
                CharacterSheetId = Guid.NewGuid()
            };

            // Act
            var result = new CharacterToken(characterTokenDto);

            // Assert
            result.Type.ShouldBe(TokenType.Character);
            result.CharacterSheetId.ShouldBe(characterTokenDto.CharacterSheetId);
        }
    }
}
