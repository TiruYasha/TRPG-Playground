using Domain.Domain;
using Domain.Domain.JournalItems;
using Domain.RepositoryInterfaces;
using Domain.RequestModels.Journal;
using Domain.RequestModels.Journal.JournalItems;
using Domain.ServiceInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Threading.Tasks;

namespace Service.Test
{
    [TestClass]
    public class JournalServiceTest
    {
        private IJournalService sut;

        private Mock<IGameRepository> gameRepository;

        [TestInitialize]
        public void Initialize()
        {
            gameRepository = new Mock<IGameRepository>();

            sut = new JournalService(gameRepository.Object);
        }

        [TestMethod]
        public async Task AddFolderToGameJournal()
        {
            var model = new AddJournalItemModel
            {
                JournalItem = new JournalFolderModel
                {
                    Name = "folder"
                },
                ParentFolderId = Guid.NewGuid()
            };

            var gameId = Guid.NewGuid();

            var userId = Guid.NewGuid();

            var game = new Mock<Game>();

            var folder = new JournalFolder();

            gameRepository.Setup(g => g.GetGameByIdAsync(gameId)).ReturnsAsync(game.Object);
            game.Setup(g => g.AddJournalItemAsync(model, userId)).ReturnsAsync(folder);

            var result = await sut.AddJournalItemToGameAsync(model, gameId, userId);

            result.ShouldBe(folder);
        }
    }
}
