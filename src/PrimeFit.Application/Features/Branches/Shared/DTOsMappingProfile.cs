using AutoMapper;
using NetTopologySuite.Geometries;
using PrimeFit.Application.Features.Branches.Shared.DTOS;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.ValueObjects;

namespace PrimeFit.Application.Features.Branches.Shared
{
    public class DTOsMappingProfile : Profile
    {
        public DTOsMappingProfile()
        {
            CreateMap<BranchImage, ImageDto>();
            CreateMap<Governorate, GovernorateDto>();
            CreateMap<GeoLocation, CoordinatesDto>();
            CreateMap<Branch, LocationDto>();
            CreateMap<BranchWorkingHour, WorkingHoursDTO>();
            CreateMap<Package, PackageDTO>();

            CreateMap<Point, CoordinatesDto>()
               .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Y))
               .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.X));

        }
    }
}
