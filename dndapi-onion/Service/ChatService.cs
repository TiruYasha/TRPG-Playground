using DataAccess;
using Domain.Domain;
using Domain.RequestModels.Chat;
using Domain.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service
{
    public class ChatService : Service, IChatService
    {
        public ChatService(DbContextOptions<DndContext> options) : base(options)
        {
        }

        public async Task<ChatMessage> AddMessageToChatAsync(SendMessageModel model, Guid userId)
        {
            var game = await context.Games.Include(g => g.Owner).Include(g => g.Players).FilterByGameId(model.GameId).FirstOrDefaultAsync();

            var chatMessage = await game.AddChatMessageAsync(model.Message, model.CustomUsername, userId);

            await context.SaveChangesAsync();

            return chatMessage;
        }

        public async Task<IList<ChatMessage>> GetAllMessagesAsync(Guid gameId)
        {
            //TODO write good code for this with lazy loading
            var game = await context.Games.Include(g => g.ChatMessages).ThenInclude(c => c.Command).FilterByGameId(gameId).FirstOrDefaultAsync();

            return game.ChatMessages.ToList();
        }
    }
}
