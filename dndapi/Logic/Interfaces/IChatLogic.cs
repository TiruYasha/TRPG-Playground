using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Logic
{
    public interface IChatLogic
    {
        Task AddMessageToGameAsync(Guid gameId, ChatMessageModel message);
        Task<List<ChatMessageModel>> GetChatMessagesForGameAsync(Guid gameId);
    }
}