using System;
using System.Threading.Tasks;
using Domain.Domain.PlayArea;
using Domain.Dto.Shared;

namespace Domain.Test.Domain.PlayArea
{
    public class TokenMock : Token
    {
        public TokenMock(TokenDto dto) : base(dto, TokenType.Default)
        {
        }
    }
}
