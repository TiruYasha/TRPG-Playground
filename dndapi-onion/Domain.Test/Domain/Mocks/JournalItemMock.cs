using Domain.Domain.JournalItems;
using System;
using System.Collections.Generic;

namespace Domain.Test.Domain.Mocks
{
    internal class JournalItemMock : JournalItem
    {
        public JournalItemMock(JournalItemType type, string name, Guid gameId, string imagePath, IList<Guid> canSee, IList<Guid> canEdit) : base(type, name, gameId, imagePath, canSee, canEdit)
        {
        }
    }
}
