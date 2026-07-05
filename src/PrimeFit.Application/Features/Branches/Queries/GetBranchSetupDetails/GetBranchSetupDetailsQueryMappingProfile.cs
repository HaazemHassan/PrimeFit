using AutoMapper;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchSetupDetails
{
    public class GetBranchSetupDetailsQueryMappingProfile : Profile
    {
        public GetBranchSetupDetailsQueryMappingProfile()
        {

            CreateMap<Branch, BranchLocationDetailsDto>();
            CreateMap<Branch, BranchBusinessDetailsDto>();


            CreateMap<Branch, GetBranchSetupDetailsQueryResponse>()
                .ForMember(dest => dest.BusinessDetails, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.WorkingHours, opt => opt.MapFrom(src => src.WorkingHours))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Where(i => i.Status == BranchImageStatus.Active)));



        }
    }
}
