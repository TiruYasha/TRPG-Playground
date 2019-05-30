using AutoMapper;
using Domain.Domain;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RestApi.Hubs;
using RestApi.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Dto.RequestDto.Chat;
using Domain.Dto.ReturnDto.Chat;
using Domain.Dto.ReturnDto.Chat.CommandResults;

namespace RestApi
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "IsGamePlayer")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService chatService;
        private readonly IJwtReader jwtReader;
        private readonly IMapper mapper;
        private readonly IHubContext<GameHub> hubContext;

        public ChatController(IChatService chatService, IJwtReader jwtReader, IMapper mapper, IHubContext<GameHub> hubContext)
        {
            this.chatService = chatService;
            this.jwtReader = jwtReader;
            this.mapper = mapper;
            this.hubContext = hubContext;
        }
     
        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllChatMessages()
        {
            var gameId = jwtReader.GetGameId();
            var messages = await chatService.GetAllMessagesAsync(gameId);
            var mappedMessages = mapper.Map<IList<ChatMessage>, IList<ReceiveMessageModel>>(messages);

            return Ok(mappedMessages);
        }

        [HttpPost]
        [Route("SendMessage")]
        public async Task SendMessage(SendMessageModel messageModel)
        {
            var userId = jwtReader.GetUserId();
            try
            {
                var chatMessage = await chatService.AddMessageToChatAsync(messageModel, userId);

                var message = mapper.Map<ChatMessage, ReceiveMessageModel>(chatMessage);

                await hubContext.Clients.Group(messageModel.GameId.ToString()).SendAsync("ChatMessageSent", message);
            }
            catch (Exception ex)
            {
                var message = new ReceiveMessageModel
                {
                    CommandResult = new UnrecognizedCommandResult(),
                    Message = ex.Message
                };

                await hubContext.Clients.User(userId.ToString()).SendAsync("ChatMessageSent", message);
            }
        }
    }
}
