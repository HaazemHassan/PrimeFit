using AutoMapper;
using PrimeFit.Application.Features.BranchPackages.Commands.UpdatePackage;

namespace PrimeFit.Api.Requests.BranchPackages.UpdatePackage {
    public class UpdatePackageRequestMappingProfile : Profile
    {
        public UpdatePackageRequestMappingProfile()
        {
            CreateMap<UpdatePackageRequest, UpdatePackageCommand>();
        }
    }
}
