using AutoMapper;
using Domain.Domain.JournalItems;
using Domain.RequestModels.Journal;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RestApi.Models.Journal;
using RestApi.Models.Journal.JournalItems;
using RestApi.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestApi.Hubs
{
    [Authorize]
    public class JournalHub : Hub
    {
        private readonly IJournalService journalService;
        private readonly IJwtReader jwtReader;
        private readonly IMapper mapper;

        public JournalHub(IJournalService journalService, IJwtReader jwtReader, IMapper mapper)
        {
            this.journalService = journalService;
            this.jwtReader = jwtReader;
            this.mapper = mapper;
        }

        public async Task AddJournalFolderAsync(AddJournalFolderModel model)
        {
            //TODO Identity check to see if the player/ownerjoined the game
            var userId = jwtReader.GetUserId();

            var journalFolder = await journalService.AddJournalFolderToGameAsync(model, userId);

            var message = mapper.Map<JournalFolder, AddedJournalFolderModel>(journalFolder);
            message.ParentId = model.ParentFolderId;

            await Clients.Caller.SendAsync("AddedJournalFolder", message);
        }

        public async Task AddToGroup(Guid gameId)
        {
            //TODO Identity check to see if the player/ownerjoined the game
            var result = await journalService.GetAllJournalItemsAsync(gameId);

            await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());

            var result2 = mapper.Map<ICollection<JournalItem>, ICollection<JournalItemModel>>(result);

            await Clients.Caller.SendAsync("AddedToGroup", result2);
        }
    }
}
