using Domain.Domain.JournalItems;
using Domain.RepositoryInterfaces;
using Domain.RequestModels.Journal;
using Domain.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class JournalService : IJournalService
    {
        private readonly IGameRepository gameRepository;

        public JournalService(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }

        public async Task<JournalItem> AddJournalFolderToGameAsync(AddFolderModel model, Guid userId)
        {
            var game = await gameRepository.GetGameByIdAsync(model.GameId);

            var result = await game.AddJournalFolderAsync(model, userId);

            return result;
        }
    }
}
