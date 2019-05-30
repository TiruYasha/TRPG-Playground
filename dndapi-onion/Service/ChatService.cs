using DataAccess;
using Domain.Domain;
using Domain.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Dto.RequestDto.Chat;

namespace Service
{
    public class ChatService : IChatService
    {
        private readonly IRepository repository;

        public ChatService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<ChatMessage> AddMessageToChatAsync(SendMessageModel model, Guid userId)
        {
            var game = await repository.Games.Include(g => g.Owner).Include(g => g.Players).FilterById(model.GameId).FirstOrDefaultAsync();

            var chatMessage = await game.AddChatMessageAsync(model.Message, model.CustomUsername, userId);

            await repository.Commit();

            return chatMessage;
        }

        public async Task<IList<ChatMessage>> GetAllMessagesAsync(Guid gameId)
        {
            //TODO write good code for this with lazy loading
            var game = await repository.Games.Include(g => g.ChatMessages).ThenInclude(c => c.Command).FilterById(gameId).FirstOrDefaultAsync();

            return game.ChatMessages.ToList();
        }
    }
}
