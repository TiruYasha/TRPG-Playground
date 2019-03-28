using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Database.JournalItems;
using Models.JournalItems;
using Repository;

namespace Logic
{
    public class JournalLogic : IJournalLogic
    {
        private IGameRepository _gameRepository;
        private IMapper _mapper;

        public JournalLogic(IGameRepository gameRepository, IMapper mapper)
        {
            _gameRepository = gameRepository;
            _mapper = mapper;
        }

        public async Task AddJournalItemToGameAsync(Guid gameId, JournalItemModel journalItemModel)
        {
            var game = await _gameRepository.FindGameByIdAsync(gameId);

            journalItemModel.Id = Guid.NewGuid();
            var journalItem = _mapper.Map<JournalItemModel, JournalItem>(journalItemModel);
                        
            game.Journal.JournalItems.Add(journalItem);

            await _gameRepository.UpdateGameAsync(game);
        }

        public async Task<List<JournalItemModel>> GetAllJournalItemsAsync(Guid gameId)
        {
            var game =  await _gameRepository.FindGameByIdAsync(gameId);

            var journalItems = _mapper.Map<List<JournalItem>, List<JournalItemModel>>(game.Journal.JournalItems);

            return journalItems;
        }
    }
}