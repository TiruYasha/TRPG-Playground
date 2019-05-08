using Domain.Domain;
using Domain.RequestModels.Chat;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.ServiceInterfaces
{
    public interface IChatService
    {
        Task<ChatMessage> AddMessageToChatAsync(SendMessageModel model, Guid userId);
        Task<IList<ChatMessage>> GetAllMessagesAsync(Guid gameId);
    }
}
