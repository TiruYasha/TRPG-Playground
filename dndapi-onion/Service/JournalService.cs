using Domain.Domain.JournalItems;
using Domain.RepositoryInterfaces;
using Domain.RequestModels.Journal;
using Domain.ServiceInterfaces;
using System;
using System.Collections.Generic;
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

        public async Task<JournalItem> AddJournalItemToGameAsync(AddJournalItemModel model, Guid gameId, Guid userId)
        {
            var game = await gameRepository.GetGameByIdAsync(gameId);

            var result = await game.AddJournalItemAsync(model, userId);

            await gameRepository.UpdateGameAsync(game);

            return result;
        }

        public async Task<ICollection<JournalItem>> GetAllJournalItemsAsync(Guid gameId)
        {
            var game = await gameRepository.GetGameByIdAsync(gameId);

            return game.JournalItems;
        }
    }
}
