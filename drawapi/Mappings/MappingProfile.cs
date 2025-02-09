using AutoMapper;
using drawapi.Data.Dtos;
using drawapi.Data.Models;

namespace drawapi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Draw, DrawDTO>(); 
            CreateMap<Group, GroupDTO>(); 
            CreateMap<Team, TeamDTO>();

            CreateMap<Group, GroupDTO>()
                .ForMember(dest => dest.Teams, opt => opt.MapFrom(src => src.GroupTeams.Select(gt => gt.Team).ToList()));

            CreateMap<Draw, DrawDTO>()
                .ForMember(dest => dest.Groups, opt => opt.MapFrom(src => src.Groups));
        }
    }
}
