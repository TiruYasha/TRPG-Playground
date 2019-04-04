using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RestApi.Models.Chat;
using System;
using System.Threading.Tasks;

namespace RestApi.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {

        public ChatHub()
        {
        }

        public async Task SendMessageToGroup(Guid gameId, ChatMessageModel message)
        {
            //await _chatLogic.AddMessageToGameAsync(gameId, message);
            await Clients.Group(gameId.ToString()).SendAsync("ReceiveMessage", message);
        }

        public async Task AddToGroup(Guid gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
        }
    }
}
