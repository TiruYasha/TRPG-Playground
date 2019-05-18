using System;
using AutoMapper;
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

        [HttpGet]
        [Route("folder/{parentFolderId}/item")]
        public async Task<IActionResult> GetJournalItemsForParentFolder(Guid? parentFolderId)
        {
            var gameId = jwtReader.GetGameId();
            var userId = jwtReader.GetUserId();

            var result = await journalService.GetJournalItemsForParentFolderId(userId, gameId, parentFolderId);

            return Ok(result);
        }

        [HttpGet]
        [Route("item")]
        public async Task<IActionResult> GetRootJournalItems()
        {
            var gameId = jwtReader.GetGameId();
            var userId = jwtReader.GetUserId();

            var result = await journalService.GetJournalItemsForParentFolderId(userId, gameId, null);

            return Ok(result);
        }


        [HttpPost]
        [Route("AddJournalItem")]
        public async Task<IActionResult> AddJournalItemAsync([FromBody] AddJournalItemDto dto)
        {
            var userId = jwtReader.GetUserId();
            var gameId = jwtReader.GetGameId();

            var journalItem = await journalService.AddJournalItemToGame(dto, gameId);

            await hubContext.Clients.User(userId.ToString()).SendAsync("JournalItemAdded", journalItem);

            return Ok(journalItem);
        }
    }
}
