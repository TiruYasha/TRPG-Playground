using Domain.Domain;
using Domain.RepositoryInterfaces;
using Domain.RequestModels.Chat;
using Domain.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service
{
    public class ChatService : IChatService
    {
        private IGameRepository gameRepository;

        public ChatService(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }

        public async Task<ChatMessage> AddMessageToChatAsync(SendMessageModel model, Guid userId)
        {
            var game = await gameRepository.GetGameByIdAsync(model.GameId);

            var chatMessage = await game.AddChatMessageAsync(model.Message, model.CustomUsername, userId);

            await gameRepository.UpdateGameAsync(game);

            return chatMessage;
        }

        public async Task<IList<ChatMessage>> GetAllMessagesAsync(Guid gameId)
        {
            //TODO write good code for this with lazy loading
            var game = await gameRepository.GetGameByIdAsync(gameId);

            return game.ChatMessages.ToList();
        }
    }
}
