using AutoMapper;
using PrimeFit.Application.Features.BranchPackages.Commands.AddPackage;

namespace PrimeFit.API.Requests.Branches
{
    public class AddPackageRequestMappingProfile : Profile
    {
        public AddPackageRequestMappingProfile()
        {
            CreateMap<AddPackageRequest, AddPackageCommand>();
        }
    }
}
