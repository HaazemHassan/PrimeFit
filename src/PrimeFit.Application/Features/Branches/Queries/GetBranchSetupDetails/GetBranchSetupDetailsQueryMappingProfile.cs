using AutoMapper;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchSetupDetails
{
    public class GetBranchSetupDetailsQueryMappingProfile : Profile
    {
        public GetBranchSetupDetailsQueryMappingProfile()
        {

            CreateMap<Branch, BranchLocationDetailsDto>();
            CreateMap<Branch, BranchBussinessDetailsDto>();


            CreateMap<Branch, GetBranchSetupDetailsQueryResponse>()
                .ForMember(dest => dest.BussinessDetails, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.WorkingHours, opt => opt.MapFrom(src => src.WorkingHours))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));



        }
    }
}
