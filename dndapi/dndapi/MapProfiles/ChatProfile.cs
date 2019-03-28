using AutoMapper;
using Database;
using Database.JournalItems;
using Models;
using Models.JournalItems;

namespace DndApi.MapProfiles
{
    public class ChatProfile : Profile
    {
        public ChatProfile()
        {
            CreateMap<ChatMessage, ChatMessageModel>();
            CreateMap<DndUser, string>().ConvertUsing(u => u.Email);
            CreateMap<CommandResult, CommandResultModel>()
                .Include<NormalRollCommandResult, NormalRollCommandResultModel>();
            CreateMap<NormalRollCommandResult, NormalRollCommandResultModel>();
            CreateMap<JournalItem, JournalItemModel>()
                .Include<Handout, HandoutModel>();
            CreateMap<Handout, HandoutModel>();
        }
    }
}
