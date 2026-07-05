using AutoMapper;
using PrimeFit.Application.Features.Branches.Queries.GetBranchSetupDetails;

namespace PrimeFit.Api.Requests.Branches.GetBranchSetupDetailsRequest {
    public class GetBranchSetupDetailsRequestMappingProfile : Profile
    {
        public GetBranchSetupDetailsRequestMappingProfile()
        {
            CreateMap<GetBranchSetupDetailsRequest, GetBranchSetupDetailsQuery>();
        }
    }
}
