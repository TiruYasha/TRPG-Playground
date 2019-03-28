using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JournalController : ControllerBase
    {
        private readonly IJournalLogic _journalLogic;

        public JournalController(IJournalLogic journalLogic)
        {
            _journalLogic = journalLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllJournalItemsByGameId(Guid gameId)
        {
            var journalItems = await _journalLogic.GetAllJournalItemsAsync(gameId);

            return Ok(journalItems);
        }
    }
}