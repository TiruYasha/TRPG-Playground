using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Domain.JournalItems;
using Domain.RequestModels.Journal;
using Domain.ReturnModels.Journal;
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
            var (userId, gameId) = GetUserIdAndGameId();

            var (journalItem, canSee)= await journalService.AddJournalItemToGame(dto, gameId);

            if (journalItem.Type == JournalItemType.Folder)
            {
                await hubContext.Clients.Group(gameId.ToString()).SendAsync("JournalItemAdded", journalItem);
            }
            else
            {
                await hubContext.Clients.User(userId.ToString()).SendAsync("JournalItemAdded", journalItem);

                await SendMessageToPlayers(canSee, journalItem);
            }

            return Ok(journalItem);
        }

        [HttpPost]
        [Route("{journalItemId}/image")]
        public async Task<IActionResult> UploadImageForJournalItem(Guid journalItemId)
        {
            // TODO validation
            var gameId = jwtReader.GetGameId();
            var file = Request.Form.Files.FirstOrDefault();

            var result = await journalService.UploadImage(file, gameId, journalItemId);

            return Ok(result);
        }

        [HttpGet]
        [Route("/game/{gameId}/{journalItemId}/image")]
        [AllowAnonymous]
        public async Task<IActionResult> GetThumbnailForJournalItem(Guid gameId, Guid journalItemId)
        {
            // TODO validation
            var imageInBytes = await journalService.GetImage(gameId, journalItemId, true);

            return File(imageInBytes, "image/jpeg");
        }

        private (Guid userId, Guid gameId) GetUserIdAndGameId()
        {
            var userId = jwtReader.GetUserId();
            var gameId = jwtReader.GetGameId();
            return (userId, gameId);
        }

        private async Task SendMessageToPlayers(IEnumerable<Guid> canSee, JournalItemTreeItemDto journalItem)
        {
            foreach (var playerId in canSee)
            {
                await hubContext.Clients.User(playerId.ToString()).SendAsync("JournalItemAdded", journalItem);
            }
        }
    }
}