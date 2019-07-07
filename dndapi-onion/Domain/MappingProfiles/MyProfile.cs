using AutoMapper;
using Domain.Domain;
using Domain.Domain.Commands;
using Domain.Domain.JournalItems;
using Domain.Domain.Layers;
using Domain.Dto.ReturnDto.Chat;
using Domain.Dto.ReturnDto.Chat.CommandResults;
using Domain.Dto.ReturnDto.Game;
using Domain.Dto.ReturnDto.Journal;
using Domain.Dto.Shared;


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

            JournalItemsMapConfig();

            CreateMap<GamePlayer, GetPlayersModel>().ForMember(dest => dest.Username, opt => opt.MapFrom(from => from.User.UserName));
            CreateMap<User, GetPlayersModel>().ForMember(dest => dest.UserId, opt => opt.MapFrom(from => from.Id));

            CreateMap<Layer, LayerDto>();
            CreateMap<LayerGroup, LayerDto>();
        }

        private void JournalItemsMapConfig()
        {
            CreateMap<JournalItem, JournalItemDto>()
                .Include<JournalFolder, JournalFolderDto>()
                .Include<JournalHandout, JournalHandoutDto>()
                .Include<JournalCharacterSheet, JournalCharacterSheetDto>();

            CreateMap<JournalFolder, JournalFolderDto>();
            CreateMap<JournalHandout, JournalHandoutDto>();
            CreateMap<JournalCharacterSheet, JournalCharacterSheetDto>();

            CreateMap<JournalItem, JournalItemTreeItemDto>();
            CreateMap<JournalFolder, JournalItemTreeItemDto>();
            CreateMap<JournalHandout, JournalItemTreeItemDto>();
            CreateMap<JournalCharacterSheet, JournalItemTreeItemDto>();
        }
    }
}
