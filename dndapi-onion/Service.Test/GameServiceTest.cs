using Domain.Domain;
using Domain.RepositoryInterfaces;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Service.Test.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Test
{
    [TestClass]
    public class GameServiceTest
    {
        private IGameService sut;

        private Mock<IGameRepository> gameRepository;
        private Mock<ILogger<GameService>> logger;
        private Mock<UserManager<User>> userManager;

        [TestInitialize]
        public void Initialize()
        {
            gameRepository = new Mock<IGameRepository>();
            logger = new Mock<ILogger<GameService>>();
            userManager = UserManagerMockFactory.GetUserManagerMock();

            sut = new GameService(gameRepository.Object, userManager.Object, logger.Object);
        }

        [TestMethod]
        public async Task CreateGameCreatesTheGame()
        {
            var gameName = "testing";
            var ownerId = new Guid("ce95484d-8ea5-4437-b649-29c4be63ae33");

            var user = new User
            {
                Id = ownerId,
                Email = "test@test.nl",
            };

            userManager.SetupGet(s => s.Users).Returns(new List<User>() { user }.AsQueryable());
            gameRepository.Setup(s => s.CreateGameAsync(It.Is<Game>(g => g.Owner == user && g.Name == gameName)))
                .Returns(Task.CompletedTask).Verifiable();

            await sut.CreateGameAsync(gameName, ownerId);

            gameRepository.VerifyAll();
        }
    }
}
