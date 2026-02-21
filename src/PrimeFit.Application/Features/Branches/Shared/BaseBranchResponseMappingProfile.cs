using AutoMapper;
using PrimeFit.Application.Features.Branches.Shared;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Users.Mapping;

public partial class BaseBranchResponseMappingProfile : Profile
{
    public BaseBranchResponseMappingProfile()
    {
        CreateMap<Branch, BaseBranchResponse>();

    }
}
