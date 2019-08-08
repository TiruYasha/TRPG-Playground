using Domain.Domain.PlayArea;
using Domain.Dto.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

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
    }
}
