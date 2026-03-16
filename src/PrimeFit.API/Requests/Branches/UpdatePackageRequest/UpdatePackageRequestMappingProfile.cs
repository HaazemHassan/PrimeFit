using AutoMapper;
using PrimeFit.Application.Features.BranchPackages.Commands.UpdatePackage;

namespace PrimeFit.API.Requests.Branches
{
    public class UpdatePackageRequestMappingProfile : Profile
    {
        public UpdatePackageRequestMappingProfile()
        {
            CreateMap<UpdatePackageRequest, UpdatePackageCommand>();
        }
    }
}
