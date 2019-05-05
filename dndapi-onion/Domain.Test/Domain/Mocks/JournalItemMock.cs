using Domain.Domain;
using Domain.Domain.JournalItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Test.Domain.Mocks
{
    internal class JournalItemMock : JournalItem
    {
        public JournalItemMock(JournalItemType type, string name, string imagePath, IList<User> canSee, IList<User> canEdit) : base(type, name, imagePath, canSee, canEdit)
        {
        }
    }
}
