using AutoMapper;
using Domain.Domain;
using Domain.Domain.Commands;
using Domain.Domain.JournalItems;
using Domain.ReturnModels.Chat;
using Domain.ReturnModels.Chat.CommandResults;
using Domain.ReturnModels.Game;
using Domain.ReturnModels.Journal;
using Domain.ReturnModels.Journal.JournalItems;

namespace Domain.MappingProfiles
{
    public class MyProfile : Profile
    {
        public MyProfile()
        {
            CreateMap<Game, GameModel>();
            CreateMap<Game, GameCatalogItemModel>();

            CreateMap<ChatMessage, ReceiveMessageModel>().ForMember(dest => dest.CommandResult, opt => opt.MapFrom(from => from.Command));
            CreateMap<Command, CommandResult>()
                .Include<DefaultCommand, DefaultCommandResult>()
                .Include<NormalDiceRollCommand, NormalDiceRollCommandResult>();
            CreateMap<DefaultCommand, DefaultCommandResult>();
            CreateMap<NormalDiceRollCommand, NormalDiceRollCommandResult>();

            CreateMap<JournalFolder, JournalItemTreeItemDto>();
            CreateMap<JournalHandout, JournalItemTreeItemDto>();

            CreateMap<JournalItem, JournalItemModel>()
                .Include<JournalFolder, JournalFolderModel>();

            CreateMap<JournalFolder, JournalFolderModel>();

            CreateMap<GamePlayer, GetPlayersModel>().ForMember(dest => dest.Username, opt => opt.MapFrom(from => from.User.UserName));
            CreateMap<User, GetPlayersModel>().ForMember(dest => dest.UserId, opt => opt.MapFrom(from => from.Id));
        }
    }
}
