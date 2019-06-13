﻿using System;

namespace Domain.Dto.Shared
{
    public class JournalCharacterSheetDto : JournalItemDto
    {
        public string Description { get; set; }
        public string OwnerNotes { get; set; }
        public Guid? ImageId { get; set; }

        public JournalCharacterSheetDto()
        {
            Type = Domain.JournalItems.JournalItemType.CharacterSheet;
        }
    }
}