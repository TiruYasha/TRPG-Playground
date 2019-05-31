using System;
using Domain.Dto.RequestDto.Journal;

namespace Domain.Domain.JournalItems
{
    public static class JournalItemFactory
    {
        public static JournalItem Create(AddJournalItemDto dto, Guid gameId)
        {
            switch (dto.JournalItem.Type)
            {
                case JournalItemType.Folder:
                    return new JournalFolder(dto, gameId);
                case JournalItemType.Handout:
                    return new JournalHandout(dto, gameId);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
