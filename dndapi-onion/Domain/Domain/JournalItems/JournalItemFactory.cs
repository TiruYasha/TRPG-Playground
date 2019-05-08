using Domain.RequestModels.Journal;
using System;

namespace Domain.Domain.JournalItems
{
    public static class JournalItemFactory
    {
        public static JournalItem Create(AddJournalItemModel model, Guid gameId)
        {
            switch (model.JournalItem.Type)
            {
                case JournalItemType.Folder:
                    return new JournalFolder(model, gameId);
                case JournalItemType.Handout:
                    return new JournalHandout(model, gameId);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
