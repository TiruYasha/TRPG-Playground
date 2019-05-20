using System;
using System.Collections.Generic;
using Domain.Domain.JournalItems;

namespace Domain.Mocks
{
    public class JournalItemMock : JournalItem
    {
        public JournalItemMock()
        {

        }

        public JournalItemMock(JournalItemType type, string name, Guid gameId, Guid? imageId, IList<Guid> canSee, IList<Guid> canEdit) : base(type, name, gameId, imageId, canSee, canEdit)
        {
        }
    }
}
