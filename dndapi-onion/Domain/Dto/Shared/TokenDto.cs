using Domain.Domain.PlayArea;
using System;

namespace Domain.Dto.Shared
{
    public class TokenDto
    {
        public Guid Id { get; set; }
        public int Y { get; set; }
        public int X { get; set; }
        public TokenType Type { get; set; }
    }

    public class CharacterTokenDto : TokenDto
    {
        public Guid CharacterSheetId { get; set; }
    }
}
