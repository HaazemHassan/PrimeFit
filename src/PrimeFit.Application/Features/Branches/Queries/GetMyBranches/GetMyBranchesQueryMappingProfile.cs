using AutoMapper;
using PrimeFit.Application.Features.Users.Queries.GetUsersPaginated;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.ValueObjects;

namespace PrimeFit.Application.Features.Branches.Queries.GetMyBranches
{
    public class GetMyBranchesQueryMappingProfile : Profile
    {
        public GetMyBranchesQueryMappingProfile()
        {
            CreateMap<BranchImage, ImageDto>();
            CreateMap<Governorate, GovernorateDto>();

            CreateMap<GeoLocation, CoordinatesDto>()
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude));

            CreateMap<Branch, LocationDto>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Governorate, opt => opt.MapFrom(src => src.Governorate))
                .ForMember(dest => dest.Coordinates, opt => opt.MapFrom(src => src.Location));

            CreateMap<Branch, GetMyBranchesQueryResponse>()
                .ForMember(dest => dest.Logo, opt => opt.MapFrom(src => src.Images
                   .Where(i => i.Type == BranchImageType.Logo)
                   .FirstOrDefault()))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src));
        }
    }
}



