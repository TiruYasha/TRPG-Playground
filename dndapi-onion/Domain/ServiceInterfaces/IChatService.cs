using Domain.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Dto.RequestDto.Chat;

namespace Domain.ServiceInterfaces
{
    public interface IChatService
    {
        Task<ChatMessage> AddMessageToChatAsync(SendMessageModel model, Guid userId);
        Task<IList<ChatMessage>> GetAllMessagesAsync(Guid gameId);
    }
}
