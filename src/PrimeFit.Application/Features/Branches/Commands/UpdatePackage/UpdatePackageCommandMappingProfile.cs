using AutoMapper;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Branches.Commands.UpdatePackage
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
