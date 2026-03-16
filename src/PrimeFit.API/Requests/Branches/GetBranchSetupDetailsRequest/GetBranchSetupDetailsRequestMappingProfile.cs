using AutoMapper;
using PrimeFit.Application.Features.Branches.Queries.GetBranchSetupDetails;

namespace PrimeFit.API.Requests.Branches
{
    public class GetBranchSetupDetailsRequestMappingProfile : Profile
    {
        public GetBranchSetupDetailsRequestMappingProfile()
        {
            CreateMap<GetBranchSetupDetailsRequest, GetBranchSetupDetailsQuery>();
        }
    }
}
