//using AutoMapper;
//using PrimeFit.Domain.Common.Enums;
//using PrimeFit.Domain.Entities;

//namespace PrimeFit.Application.Features.Branches.Queries.GetNearbyBranches
//{
//    public class GetNearbyBranchesQueryMappingProfile : Profile
//    {
//        public GetNearbyBranchesQueryMappingProfile()
//        {

//            CreateMap<Branch, GetNearbyBranchesQueryResponse>()
//                .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => src.Images
//                   .Where(i => i.Type == BranchImageType.Logo).Select(i => i.Url)
//                   .FirstOrDefault()))
//                .ForMember(dest => dest.Governate, opt => opt.MapFrom(src => src.Governorate!.Name))
//                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Coordinates!.Y))
//                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Coordinates!.X))
//                .ForMember(dest => dest.DistanceInKm, opt => opt.MapFrom(src => src.Coordinates!.Distance))


//        }
//    }
//}



