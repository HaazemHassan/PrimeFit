using AutoMapper;
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
        }

    }
}
