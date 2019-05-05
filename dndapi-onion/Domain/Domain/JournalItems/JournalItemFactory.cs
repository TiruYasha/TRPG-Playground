using Domain.RequestModels.Journal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Domain.JournalItems
{
    public static class JournalItemFactory
    {
        public static JournalItem Create(AddJournalItemModel model)
        {
            switch (model.JournalItem.Type)
            {
                case JournalItemType.Folder:
                    return new JournalFolder(model);
                case JournalItemType.Handout:
                    return new JournalHandout(model);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
