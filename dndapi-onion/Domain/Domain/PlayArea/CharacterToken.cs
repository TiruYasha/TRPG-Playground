using Domain.Domain.JournalItems;
using Domain.Dto.Shared;
using System;

namespace Domain.Domain.PlayArea
{
    public class CharacterToken : Token
    {
        public Guid CharacterSheetId { get; private set; }
        public virtual JournalCharacterSheet CharacterSheet { get; private set; }

        private CharacterToken()  { }

        public CharacterToken(CharacterTokenDto dto) : base(dto, TokenType.Character)
        {
            CharacterSheetId = dto.CharacterSheetId;
        }
    }
}
