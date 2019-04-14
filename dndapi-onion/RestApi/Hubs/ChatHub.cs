using AutoMapper;
using Domain.Domain;
using Domain.Exceptions;
using Domain.RequestModels.Chat;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RestApi.Models.Chat;
using RestApi.Utilities;
using System;
using System.Threading.Tasks;

namespace RestApi.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatService chatService;
        private readonly IJwtReader jwtReader;
        private readonly IMapper mapper;

        public ChatHub(IChatService chatService, IJwtReader jwtReader, IMapper mapper)
        {
            this.chatService = chatService;
            this.jwtReader = jwtReader;
            this.mapper = mapper;
        }

        public async Task SendMessageToGroup(SendMessageModel messageModel)
        {
            var userId = jwtReader.GetUserId();
            try
            {
                var chatMessage = await chatService.AddMessageToChatAsync(messageModel, userId);

                var message = mapper.Map<ChatMessage, ReceiveMessageModel>(chatMessage);

                await Clients.Group(messageModel.GameId.ToString()).SendAsync("ReceiveMessage", message);
            }
            catch(Exception ex)
            {
                var message = new ReceiveMessageModel
                {
                    CommandResult = new Models.Chat.CommandResults.UnrecognizedCommandResult(),
                    Message = ex.Message
                };

                await Clients.Caller.SendAsync("ReceiveMessage", message);
            }
        }

        public async Task AddToGroup(Guid gameId)
        {
            //TODO Identity check to see if the player/ownerjoined the game
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
        }
    }
}
