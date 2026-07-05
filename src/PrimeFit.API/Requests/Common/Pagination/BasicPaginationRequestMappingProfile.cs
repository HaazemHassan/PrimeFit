using AutoMapper;
using PrimeFit.Application.Features.BranchPackages.Queries.GetBranchPackages;

namespace PrimeFit.Api.Requests.Common.Pagination {
    public class BasicPaginationRequestMappingProfile : Profile
    {
        public BasicPaginationRequestMappingProfile()
        {
            CreateMap<BasicPaginationRequest, GetBranchPackagesQuery>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember is not null));
        }
    }
}
