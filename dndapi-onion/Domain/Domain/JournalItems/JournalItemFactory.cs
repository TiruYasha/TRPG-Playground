using System;
using Domain.Dto.RequestDto.Journal;
using Domain.Dto.Shared;

namespace Domain.Domain.JournalItems
{
    public static class JournalItemFactory
    {
        public static JournalItem Create(JournalItemDto dto, Guid gameId)
        {
            switch (dto.Type)
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
