using Domain.Dto.Shared;
using System;

namespace Domain.Domain.PlayArea
{
    public class Token
    {
        public Guid Id { get; private set; }
        public int Y { get; private set; }
        public int X { get; private set; }
        public TokenType Type { get; private set; }
        public Guid ImageId { get; private set; }
        public virtual Image Image { get; private set; }
        
        private Token() { }

        public Token(TokenDto dto) : this()
        {
            Id = Guid.NewGuid();
            Y = dto.Y;
            X = dto.X;
            Type = dto.Type;
            ImageId = dto.ImageId;
        }
    }
}
