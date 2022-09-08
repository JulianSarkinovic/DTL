using AutoMapper;
using DikkeTennisLijst.Core.Entities;
using DikkeTennisLijst.WebAPI.Models.Request;
using DikkeTennisLijst.WebAPI.Models.Response;

namespace DikkeTennisLijst.WebAPI.Models.Mapping
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<Player, PlayerRegisterRequestModel>()
                .ReverseMap()
                .ForMember(t => t.Id, opt => opt.Ignore())
                .ForMember(t => t.UserName, opt => opt.MapFrom(scr => scr.Email));

            CreateMap<Player, PlayerResponseModel>()
                .ForMember(t => t.Elo, opt => opt.MapFrom(scr => scr.EloRanked));

            CreateMap<Player, PlayerForProfileResponseModel>()
                .ForMember(t => t.Elo, opt => opt.MapFrom(scr => scr.EloRanked))
                .ForMember(t => t.Matches, opt => opt.MapFrom(scr => scr.Matches));

            CreateMap<Player, PlayerRequestModel>()
                .ReverseMap()
                .ForMember(t => t.Id, opt => opt.Ignore())
                .ForMember(t => t.UserName, opt => opt.MapFrom(scr => scr.Email));

            CreateMap<Comment, CommentResponseModel>();
            CreateMap<Comment, CommentRequestModel>()
                .ReverseMap()
                .ForMember(t => t.Id, opt => opt.Ignore());

            CreateMap<Match, MatchResponseModel>()
                .ForMember(t => t.PlayerOne, opt => opt.MapFrom(src => src.PlayerOne))
                .ForMember(t => t.PlayerTwo, opt => opt.MapFrom(src => src.PlayerTwo))
                .ForMember(t => t.PlayerOnePartner, opt => opt.MapFrom(src => src.PlayerOnePartner))
                .ForMember(t => t.PlayerTwoPartner, opt => opt.MapFrom(src => src.PlayerTwoPartner));

            CreateMap<Match, MatchFullResponseModel>();

            CreateMap<Match, MatchRequestModel>()
                .ReverseMap()
                .ForMember(t => t.Id, opt => opt.Ignore());

            CreateMap<EloRanked, EloResponseModel>()
                .ForMember(t => t.History, opt => opt.Ignore());

            CreateMap<Club, ClubResponseModel>()
                .ForMember(t => t.SurfacesIds, opt => opt.MapFrom<SurfaceResolver, ICollection<SurfaceClubJoin>>(scr => scr.SurfacesClubJoins));
            CreateMap<Club, ClubRequestModel>()
                .ReverseMap()
                .ForMember(t => t.Id, opt => opt.Ignore());

            CreateMap<Surface, SurfaceResponseModel>();
        }
    }

    public class SurfaceResolver : IMemberValueResolver<Club, ClubResponseModel, ICollection<SurfaceClubJoin>, int[]>
    {
        public int[] Resolve(Club source, ClubResponseModel destination, ICollection<SurfaceClubJoin> sourceMember, int[] destMember, ResolutionContext context)
        {
            return source.SurfacesClubJoins.Select(x => x.SurfaceId).ToArray();
        }
    }
}