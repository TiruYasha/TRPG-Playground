using Domain.Dto.Shared;

namespace Domain.Domain.PlayArea
{
    public static class TokenFactory
    {
        public static Token Create(TokenDto dto)
        {
            switch (dto.Type)
            {
                case TokenType.Character:
                    return new CharacterToken(dto as CharacterTokenDto);
                default:
                    return null;
            }
        }
    }
}
