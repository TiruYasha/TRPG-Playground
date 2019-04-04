using System;
using System.Threading.Tasks;
using Logic;
using Logic.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Models;

namespace dndapi.Controllers
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatLogic _chatLogic;

        public ChatHub(IChatLogic chatLogic)
        {
            _chatLogic = chatLogic;
        }

        public async Task SendMessageToGroup(Guid gameId, ChatMessageModel message)
        {
            await _chatLogic.AddMessageToGameAsync(gameId, message);
            await Clients.Group(gameId.ToString()).SendAsync("ReceiveMessage", message);
            
        }

        public async Task AddToGroup(Guid gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
        }
    }
}