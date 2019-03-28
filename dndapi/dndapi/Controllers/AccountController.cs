using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models;

namespace DndApi.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<DndUser> _userManager;
        private readonly IOptions<MyConfig> _config;

        public AccountController(UserManager<DndUser> userManager, IOptions<MyConfig> config)
        {
            _userManager = userManager;
            _config = config;
        }

        [HttpPost]
        [Route("/register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var dndUser = new DndUser
            {
                Email = model.Email,
                UserName = "testing" + Guid.NewGuid()
            };

            var result = await _userManager.CreateAsync(dndUser, model.Password);

            return Ok();
        }
            
        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login([FromBody] RegisterModel model)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Email == model.Email);

            if(user == null)
            {
                return BadRequest();
            }
            
            var result = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!result)
            {
                return BadRequest();
            }

            var token = BuildToken(user);

            var loginSuccess = new LoginSuccessModel
            {
                Token = token
            };

            return Ok(loginSuccess);
        }

        private string BuildToken(DndUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Value.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config.Value.Issuer,
                _config.Value.Issuer,
                expires: DateTime.Now.AddDays(20),
                signingCredentials: creds,
                claims: claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}