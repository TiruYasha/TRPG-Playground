using AutoMapper;
using Domain.Domain;
using RestApi.Models.Game;

namespace DependencyResolver.MappingProfiles
{
    public class MyProfile : Profile
    {
        public MyProfile()
        {
            CreateMap<Game, GameModel>();
        }
    }
}
