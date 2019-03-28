using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Database;
using Logic.Helpers;
using Logic.Utilities;
using Models;
using Repository;

namespace Logic
{
    public class ChatLogic : IChatLogic
    {
        private IGameRepository _gameRepository;
        private readonly IJwtReader _jwtReader;
        private readonly ICommandHelper _commandHelper;
        private readonly IMapper _mapper;

        public ChatLogic(IGameRepository gameRepository, IJwtReader jwtReader, ICommandHelper commandHelper, IMapper mapper)
        {
            _gameRepository = gameRepository;
            _jwtReader = jwtReader;
            _commandHelper = commandHelper;
            _mapper = mapper;
        }

        public async Task AddMessageToGameAsync(Guid gameId, ChatMessageModel message)
        {
            var game = await _gameRepository.FindGameByIdAsync(gameId);

            var userId = _jwtReader.GetUserId();
            DndUser user = GetUser(game, userId);

            var chatMessage = new ChatMessage
            {
                Id = Guid.NewGuid(),
                Game = game,
                Message = message.Message,
                User = user
            };

            if (_commandHelper.CheckIfMessageIsCommand(message.Message))
            {
                chatMessage.CommandResult = _commandHelper.RunCommand(message.Message);
            }

            await _gameRepository.AddMessageAsync(chatMessage);

            if (chatMessage.CommandResult != null)
            {
                var commandResult = new NormalRollCommandResultModel
                {
                    Result = ((NormalRollCommandResult)chatMessage.CommandResult).Result
                };

                message.CommandResult = commandResult;
            }

            message.User = user.Email;
        }

        public async Task<List<ChatMessageModel>> GetChatMessagesForGameAsync(Guid gameId)
        {
            var game = await _gameRepository.FindGameByIdAsync(gameId);
            var messages = game.Messages.Select(s => new ChatMessage
             {
                 CreatedDate = s.CreatedDate,
                 Message = s.Message,
                 CommandResult = s.CommandResult,
                 User = s.User
             })
             .OrderBy(m => m.CreatedDate).ToList();

            List<ChatMessageModel> chatMessages = _mapper.Map<List<ChatMessage>, List<ChatMessageModel>>(messages);

            return chatMessages;
        }

        private static DndUser GetUser(Game game, Guid userId)
        {
            var user = new DndUser();

            if (game.Owner?.Id == userId.ToString())
            {
                user = game.Owner;
            }
            else
            {
                var participant = game.Participants.FirstOrDefault(p => p.User.Id == userId.ToString());

                user = participant.User;
            }

            return user;
        }
    }
}