using AutoMapper;
using Domain.Domain.JournalItems;
using Domain.RequestModels.Journal;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RestApi.Models.Journal;
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
            var userId = jwtReader.GetUserId();

            var journalFolder = await journalService.AddJournalFolderToGameAsync(model, userId);

            var message = mapper.Map<JournalFolder, AddedJournalFolderModel>(journalFolder);

            await Clients.Caller.SendAsync("AddedJournalFolder", message);
        }
    }
}
