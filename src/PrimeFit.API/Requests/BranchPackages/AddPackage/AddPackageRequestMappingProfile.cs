using AutoMapper;
using PrimeFit.Application.Features.BranchPackages.Commands.AddPackage;

namespace PrimeFit.Api.Requests.BranchPackages.AddPackage {
    public class AddPackageRequestMappingProfile : Profile
    {
        public AddPackageRequestMappingProfile()
        {
            CreateMap<AddPackageRequest, AddPackageCommand>();
        }
    }
}
