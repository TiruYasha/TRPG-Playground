using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Gateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
       // private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController()//ILogger<AuthenticationController> logger)
        {
            //_logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {

            return Ok("wow");
        }
    }
}
