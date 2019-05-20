using System;
using System.Threading.Tasks;
using Domain.RequestModels.Journal;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RestApi.Hubs;
using RestApi.Utilities;

namespace RestApi
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "IsGamePlayer")]
    public class JournalController : ControllerBase
    {
        private readonly IHubContext<GameHub> hubContext;
        private readonly IJournalService journalService;
        private readonly IJwtReader jwtReader;

        public JournalController(IJournalService journalService, IJwtReader jwtReader, IHubContext<GameHub> hubContext)
        {
            this.journalService = journalService;
            this.jwtReader = jwtReader;
            this.hubContext = hubContext;
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