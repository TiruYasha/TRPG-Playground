using System;
using System.Collections.Generic;
using Domain.Domain.JournalItems;

namespace Domain.Mocks
{
    public class JournalItemMock : JournalItem
    {
        public JournalItemMock()
        {
            Permissions = new List<JournalItemPermission>();
        }

        public JournalItemMock(JournalItemType type, string name, Guid gameId, ICollection<Guid> canSee, ICollection<Guid> canEdit) : base(type, name, gameId, canSee, canEdit)
        {
        }
    }
}
