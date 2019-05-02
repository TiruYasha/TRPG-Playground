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
            switch (model.JournalItemType)
            {
                case JournalItemType.Folder:
                    return new JournalFolder(model);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
