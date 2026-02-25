using AutoMapper;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Packages.Commands.UpdatePackage
{
    public class UpdatePackageCommandMappingProfile : Profile
    {
        public UpdatePackageCommandMappingProfile()
        {
            CreateMap<Package, UpdatePackageCommandResponse>()
                .ForMember(dest => dest.PackageId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
