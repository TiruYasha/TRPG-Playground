using System;
using System.Threading.Tasks;
using Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatLogic _chatLogic;

        public ChatController(IChatLogic chatLogic)
        {
            _chatLogic = chatLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetChatMessages(Guid gameId)
        {
            var messages = await _chatLogic.GetChatMessagesForGameAsync(gameId);

            return Ok(messages);
        }
    }
}