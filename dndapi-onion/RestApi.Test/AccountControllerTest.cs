using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestApi.Models.Account;
using Shouldly;
using System;
using System.Threading.Tasks;

namespace RestApi.Test
{
    [TestClass]
    public class AccountControllerTest
    {
        private AccountController sut;

        private Mock<IAccountService> accountService;

        [TestInitialize]
        public void Initialize()
        {
            accountService = new Mock<IAccountService>();

            sut = new AccountController(accountService.Object);
        }

        [TestMethod]
        public async Task RegisterReturnsOkResultOnSuccess()
        {
            var registerModel = new RegisterModel
            {
                Email = "test@test.nl",
                Password = "password"
            };

            accountService.Setup(s => s.RegisterAsync(registerModel.Email, registerModel.Password)).Returns(Task.CompletedTask).Verifiable();

            OkResult result = await sut.RegisterAsync(registerModel) as OkResult;

            result.ShouldBeOfType<OkResult>();
            accountService.VerifyAll();
        }

        [TestMethod]
        public async Task RegisterReturnsBadRequestOnAccountServiceExceptionAsync()
        {
            var registerModel = new RegisterModel();

            accountService.Setup(s => s.RegisterAsync(null, null)).Throws(new ArgumentNullException("Test"));

            BadRequestObjectResult result = await sut.RegisterAsync(registerModel) as BadRequestObjectResult;

            result.ShouldBeOfType<BadRequestObjectResult>();
            result.Value.ShouldBe(@"Value cannot be null.
Parameter name: Test");
        }

        [TestMethod]
        public async Task LoginReturnsTokenOnSuccess()
        {
            var loginModel = new LoginModel
            {
                Email = "test@test.nl",
                Password = "password"
            };

            accountService.Setup(s => s.LoginAsync(loginModel.Email, loginModel.Password)).ReturnsAsync("token").Verifiable();

            OkObjectResult result = await sut.LoginAsync(loginModel) as OkObjectResult;

            result.ShouldBeOfType<OkObjectResult>();
            result.Value.ShouldBe("token");
            accountService.VerifyAll();
        }

        [TestMethod]
        public async Task LoginReturnsBadRequestOnAccountServiceExceptionAsync()
        {
            var loginModel = new LoginModel();

            accountService.Setup(s => s.LoginAsync(null, null)).Throws(new ArgumentNullException("Test"));

            BadRequestObjectResult result = await sut.LoginAsync(loginModel) as BadRequestObjectResult;

            result.ShouldBeOfType<BadRequestObjectResult>();
            result.Value.ShouldBe(@"Value cannot be null.
Parameter name: Test");
        }
    }
}
