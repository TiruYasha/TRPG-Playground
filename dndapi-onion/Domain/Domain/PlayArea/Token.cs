using Domain.Domain.Layers;
using Domain.Dto.Shared;
using System;

namespace Domain.Domain.PlayArea
{
    public abstract class Token
    {
        public Guid Id { get; private set; }
        public int Y { get; private set; }
        public int X { get; private set; }
        public TokenType Type { get; private set; }
        public Guid LayerId { get; private set; }
        public virtual Layer Layer { get; private set; }
        
        protected Token() { }

        public Token(TokenDto dto, TokenType type) : this()
        {
            Id = Guid.NewGuid();
            Y = dto.Y;
            X = dto.X;
            Type = type;
        }
    }
}
