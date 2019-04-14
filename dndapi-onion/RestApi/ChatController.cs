using AutoMapper;
using Domain.Domain;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApi.Models.Chat;
using RestApi.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        public ChatController(IChatService chatService, IJwtReader jwtReader, IMapper mapper)
        {
            this.chatService = chatService;
            this.jwtReader = jwtReader;
            this.mapper = mapper;
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
    }
}
