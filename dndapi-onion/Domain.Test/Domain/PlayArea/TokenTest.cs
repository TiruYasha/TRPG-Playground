using Domain.Domain.PlayArea;
using Domain.Dto.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Threading.Tasks;

namespace Domain.Test.Domain.PlayArea
{
    [TestClass]
    public class TokenTest
    {

        [TestMethod]
        public void TokenConstructor_SetsAllValues()
        {
            // Arrange
            var tokenDto = new TokenDto
            {
                Y = 10,
                X = 20,
                Type = TokenType.Default
            };

            // Act
            var result = new TokenMock(tokenDto);

            // Assert
            result.Type.ShouldBe(TokenType.Default);
            result.X.ShouldBe(tokenDto.X);
            result.Y.ShouldBe(tokenDto.Y);
            result.Id.ShouldNotBe(Guid.Empty);
        }

        [TestMethod]
        public async Task Move_MovesTokenToSpecifiedPosition()
        {
            // Arrange
            var tokenDto = new TokenDto
            {
                Y = 10,
                X = 20,
                Type = TokenType.Default
            };
            var sut = new TokenMock(tokenDto);

            var newPositionX = 40;
            var newPositionY = 50;

            // Act
            await sut.Move(newPositionX, newPositionY);

            // Assert
            sut.X.ShouldBe(newPositionX);
            sut.Y.ShouldBe(newPositionY);
        }
    }
}
