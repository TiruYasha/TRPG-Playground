using Domain.Config;
using Domain.Domain;
using Domain.Exceptions;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Test
{
    [TestClass]
    public class AccountServiceTest
    {
        private IAccountService sut;

        private Mock<UserManager<User>> userManager;
        private Mock<IOptions<TokenConfig>> tokenConfig;

        private const string issuer = "Dnd";
        private const string key = "dfsafdsafdsafadsfkjadshfkjahdkjfhadskjdasfjkdshfkjdaslhfkasf";

        [TestInitialize]
        public void Initialize()
        {
            userManager = GetUserManagerMock();
            tokenConfig = new Mock<IOptions<TokenConfig>>();
            sut = new AccountService(userManager.Object, tokenConfig.Object);

            tokenConfig.SetupGet(s => s.Value.Issuer).Returns(issuer);
            tokenConfig.SetupGet(s => s.Value.Key).Returns(key);
        }

        [TestMethod]
        public async Task RegisterRegistersTheUserViaUserManager()
        {
            var email = "testEmail";
            var password = "testpass";

            userManager.Setup(s => s.CreateAsync(It.Is<User>(u => u.Email == email && u.UserName == email), password))
                .ReturnsAsync(IdentityResult.Success)
                .Verifiable();

            await sut.RegisterAsync(email, password);

            userManager.VerifyAll();
        }

        [TestMethod]
        public async Task RegisterThrowsArgumentNullExceptionOnNullEmail()
        {
            string email = null;
            string password = "test";

            var exception = await Should.ThrowAsync<ArgumentNullException>(async () =>
            {
                await sut.RegisterAsync(email, password);
            });

            exception.ParamName.ShouldBe("Email");
        }

        [TestMethod]
        public async Task RegisterThrowsArgumentNullExceptionOnNullPassword()
        {
            string email = "email";
            string password = null;

            var exception = await Should.ThrowAsync<ArgumentNullException>(async () =>
            {
                await sut.RegisterAsync(email, password);
            });

            exception.ParamName.ShouldBe("Password");
        }

        [TestMethod]
        public async Task LoginAsyncReturnsTokenOnSuccess()
        {
            var email = "test";
            var password = "password";

            var user = new User
            {
                Email = email,
            };

            userManager.SetupGet(s => s.Users).Returns(new List<User>() { user }.AsQueryable());
            userManager.Setup(s => s.CheckPasswordAsync(user, password)).ReturnsAsync(true);

            var result = await sut.LoginAsync(email, password);

            result.StartsWith("ey").ShouldBe(true);
        }


        [TestMethod]
        public async Task LoginThrowsArgumentNullExceptionOnNullEmail()
        {
            string email = null;
            string password = "test";

            var exception = await Should.ThrowAsync<ArgumentNullException>(async () =>
            {
                await sut.LoginAsync(email, password);
            });

            exception.ParamName.ShouldBe("Email");
        }

        [TestMethod]
        public async Task LoginThrowsArgumentNullExceptionOnNullPassword()
        {
            string email = "email";
            string password = null;

            var exception = await Should.ThrowAsync<ArgumentNullException>(async () =>
            {
                await sut.LoginAsync(email, password);
            });

            exception.ParamName.ShouldBe("Password");
        }

        [TestMethod]
        public async Task LoginAsyncIfUserDoesNotExistThrowLoginException()
        {
            string email = "email";

            var exception = await Should.ThrowAsync<LoginException>(async () =>
            {
                await sut.LoginAsync(email, email);
            });

            exception.Message.ShouldBe("The user does not exist");
        }

        [TestMethod]
        public async Task LoginAsyncIfUserPasswordOrUserNameIsWrongThrowLoginException()
        {
            string email = "email";
            string password = "password";

            var user = new User
            {
                Email = email,
            };

            userManager.SetupGet(s => s.Users).Returns(new List<User>() { user }.AsQueryable());
            userManager.Setup(s => s.CheckPasswordAsync(user, password)).ReturnsAsync(false);

            var exception = await Should.ThrowAsync<LoginException>(async () =>
            {
                await sut.LoginAsync(email, password);
            });

            exception.Message.ShouldBe("The username or password is wrong");
        }

        private Mock<UserManager<User>> GetUserManagerMock()
        {
            var store = new Mock<IUserStore<User>>();
            var mgr = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<User>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<User>());

            return mgr;
        }
    }
}
