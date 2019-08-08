using Domain.Domain.PlayArea;
using Domain.Dto.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

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
                ImageId = Guid.NewGuid(),
                Type = TokenType.Default
            };

            // Act
            var result = new Token(tokenDto);

            // Assert
            result.Type.ShouldBe(TokenType.Default);
            result.X.ShouldBe(tokenDto.X);
            result.Y.ShouldBe(tokenDto.Y);
            result.ImageId.ShouldBe(tokenDto.ImageId);
            result.Id.ShouldNotBe(Guid.Empty);
        }
    }
}
