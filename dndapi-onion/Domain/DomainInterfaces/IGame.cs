using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Domain;
using Domain.Domain.JournalItems;
using Domain.Dto.RequestDto.Journal;

namespace Domain.DomainInterfaces
{
    public interface IGame
    {
        Guid Id { get; }
        string Name { get; }
        User Owner { get; }
        Guid OwnerId { get; }
        ICollection<GamePlayer> Players { get; }
        ICollection<ChatMessage> ChatMessages { get; }
        ICollection<JournalItem> JournalItems { get; }

        Task Join(User user);
        Task<bool> HasPlayerJoined(Guid userId);
        Task<bool> IsOwner(Guid ownerId);
        Task<ChatMessage> AddChatMessageAsync(string message, string customUsername, Guid userId);
        Task<JournalItem> AddJournalItemAsync(AddJournalItemDto dto);
    }
}
