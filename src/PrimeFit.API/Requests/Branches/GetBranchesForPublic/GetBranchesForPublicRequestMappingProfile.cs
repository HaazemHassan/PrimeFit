using AutoMapper;
using PrimeFit.Application.Features.Branches.Queries.GetBranchesForPublic;

namespace PrimeFit.Api.Requests.Branches.GetBranchesForPublic;

public class GetBranchesForPublicRequestMappingProfile : Profile
{
    public GetBranchesForPublicRequestMappingProfile()
    {
        CreateMap<GetBranchesForPublicRequest, GetBranchesForPublicQuery>();
    }
}
