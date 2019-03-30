using Domain.Config;
using Domain.Domain;
using Domain.Exceptions;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> userManager;
        private readonly IOptions<TokenConfig> tokenConfig;
        private readonly ILogger logger;

        public AccountService(UserManager<User> userManager, IOptions<TokenConfig> tokenConfig, ILogger<AccountService> logger)
        {
            this.userManager = userManager;
            this.tokenConfig = tokenConfig;
            this.logger = logger;
        }

        public async Task RegisterAsync(string email, string password)
        {
            CheckNullValues(email, password);

            var user = new User
            {
                Email = email,
                UserName = email
            };

            var result = await userManager.CreateAsync(user, password);

            logger.LogInformation($"The user {user.Email} has been created");
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            CheckNullValues(email, password);

            var user = userManager.Users.FirstOrDefault(u => u.Email == email);

            if (user == null) throw new LoginException("The user does not exist");

            var isValid = await userManager.CheckPasswordAsync(user, password);

            if (!isValid) throw new LoginException("The username or password is wrong");

            var token = BuildToken(user);

            logger.LogInformation($"Login for user {user.Email} successfull");

            return token;
        }

        private string BuildToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfig.Value.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(tokenConfig.Value.Issuer,
                tokenConfig.Value.Issuer,
                expires: DateTime.Now.AddDays(20),
                signingCredentials: creds,
                claims: claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void CheckNullValues(string email, string password)
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException("Email");
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("Password");
        }
    }
}
