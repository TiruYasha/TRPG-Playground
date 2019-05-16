﻿using AutoMapper;
using Domain.Domain.JournalItems;
using Domain.RequestModels.Journal;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RestApi.Hubs;
using RestApi.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.ReturnModels.Journal;
using Domain.ReturnModels.Journal.JournalItems;

namespace RestApi
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "IsGamePlayer")]
    public class JournalController : ControllerBase
    {
        private readonly IJournalService journalService;
        private readonly IJwtReader jwtReader;
        private readonly IMapper mapper;
        private readonly IHubContext<GameHub> hubContext;

        public JournalController(IJournalService journalService, IJwtReader jwtReader, IMapper mapper, IHubContext<GameHub> hubContext)
        {
            this.journalService = journalService;
            this.jwtReader = jwtReader;
            this.mapper = mapper;
            this.hubContext = hubContext;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllJournalItemsAsync()
        {
            var gameId = jwtReader.GetGameId();
            var userId = jwtReader.GetUserId();
            var result = await journalService.GetAllJournalItemsAsync(userId, gameId);

            var mappedResult = mapper.Map<ICollection<JournalItem>, ICollection<JournalItemModel>>(result);

            return Ok(mappedResult);
        }

        [HttpPost]
        [Route("AddJournalItem")]
        public async Task<IActionResult> AddJournalItemAsync([FromBody] AddJournalItemModel model)
        {
            var userId = jwtReader.GetUserId();
            var gameId = jwtReader.GetGameId();

            var journalFolder = await journalService.AddJournalItemToGameAsync(model, gameId);

            //var message = mapper.Map<JournalItem, AddedJournalItemModel>(journalFolder);
            //message.ParentId = model.ParentFolderId;

           // await hubContext.Clients.User(userId.ToString()).SendAsync("JournalItemAdded", message);

            return Ok();
        }
    }
}
