﻿using Domain.Domain.JournalItems;
using Newtonsoft.Json.Linq;
using System;
using Domain.Dto.Shared;

namespace Domain.JsonConverter
{
    public class JournalItemModelConverter : JsonCreationConverter<JournalItemDto>
    {
        protected override JournalItemDto Create(Type objectType, JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException("jObject");

            var journalItemType = jObject.Value<JournalItemType>("Type");

            if (journalItemType != JournalItemType.Folder)
            {
                return new JournalFolderDto();
            }
            else if (journalItemType != JournalItemType.Handout)
            {
                return new JournalHandoutDto();
            }
            else
            {
                return new JournalItemDto();
            }
        }
    }
}
