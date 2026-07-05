using AutoMapper;
using PrimeFit.Application.Features.Branches.Commands.CreateBranch;

namespace PrimeFit.Api.Requests.Branches.CreateBranch;

public class CreateBranchRequestMappingProfile : Profile
{
    public CreateBranchRequestMappingProfile()
    {
        CreateMap<CreateBranchRequest, CreateBranchCommand>();
    }
}
