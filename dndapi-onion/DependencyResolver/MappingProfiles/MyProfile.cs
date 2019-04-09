﻿using AutoMapper;
using Domain.Domain;
using Domain.Domain.Commands;
using Domain.Domain.JournalItems;
using RestApi.Models.Chat;
using RestApi.Models.Chat.CommandResults;
using RestApi.Models.Game;
using RestApi.Models.Journal;

namespace DependencyResolver.MappingProfiles
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

            CreateMap<JournalFolder, AddedJournalFolderModel>();
        }
    }
}
