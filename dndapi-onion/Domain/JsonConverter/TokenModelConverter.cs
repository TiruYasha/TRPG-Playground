using Domain.Domain.PlayArea;
using Domain.Dto.Shared;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.JsonConverter
{
    public class TokenModelConverter : JsonCreationConverter<TokenDto>
    {
        protected override TokenDto Create(Type objectType, JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException("jObject");
            var typeNumber = jObject.Value<int>("type");
            var journalItemType = (TokenType)typeNumber;

            if (journalItemType == TokenType.Character)
            {
                return new CharacterTokenDto();
            }
            else
            {
                return new TokenDto();
            }
        }
    }
}
