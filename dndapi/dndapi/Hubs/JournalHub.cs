using Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Models.JournalItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DndApi.Hubs
{
    //[Authorize]
    //public class JournalHub : Hub
    //{
    //    private readonly IJournalLogic _journalLogic;

    //    public JournalHub(IJournalLogic journalLogic)
    //    {
    //        _journalLogic = journalLogic;
    //    }

    //    public async Task AddJournalItemToGame(Guid gameId, JournalItemModel journalItem)
    //    {
    //        await _journalLogic.AddJournalItemToGameAsync(gameId, journalItem);
    //        await Clients.Group(gameId.ToString()).SendAsync("JournalItemAdded");
    //    }

    //    public async Task AddToGroup(Guid gameId)
    //    {
    //        await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
    //    }
    //}
}
