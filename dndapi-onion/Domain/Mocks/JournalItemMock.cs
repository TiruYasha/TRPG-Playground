using System;
using System.Collections.Generic;
using Domain.Domain.JournalItems;
using Domain.Dto.Shared;

namespace Domain.Mocks
{
    public class JournalItemMock : JournalItem
    {
        public JournalItemMock(JournalItemDto dto, Guid gameId) : base(dto, gameId)
        {
        }
    }
}
