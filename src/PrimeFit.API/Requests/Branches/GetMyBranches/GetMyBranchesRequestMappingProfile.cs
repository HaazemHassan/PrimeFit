using AutoMapper;
using PrimeFit.Application.Features.Branches.Queries.GetMyBranches;

namespace PrimeFit.Api.Requests.Branches.GetMyBranches;

public class GetMyBranchesRequestMappingProfile : Profile
{
    public GetMyBranchesRequestMappingProfile()
    {
        CreateMap<GetMyBranchesRequest, GetMyBranchesQuery>();
    }
}
