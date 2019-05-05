using AutoMapper;
using Domain.Domain.JournalItems;
using Domain.RequestModels.Journal;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RestApi.Hubs;
using RestApi.Models.Journal;
using RestApi.Models.Journal.JournalItems;
using RestApi.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            var result = await journalService.GetAllJournalItemsAsync(gameId);

            var mappedResult = mapper.Map<ICollection<JournalItem>, ICollection<JournalItemModel>>(result);

            return Ok(mappedResult);
        }

        [HttpPost]
        [Route("AddJournalFolder")]
        public async Task<IActionResult> AddJournalItemAsync([FromBody] AddJournalItemModel model)
        {
            var userId = jwtReader.GetUserId();
            var gameId = jwtReader.GetGameId();

            var journalFolder = await journalService.AddJournalItemToGameAsync(model, gameId, userId);

            var message = mapper.Map<JournalItem, AddedJournalFolderModel>(journalFolder);
            message.ParentId = model.ParentFolderId;

            await hubContext.Clients.User(userId.ToString()).SendAsync("JournalFolderAdded", message);

            return Ok();
        }
    }
}
