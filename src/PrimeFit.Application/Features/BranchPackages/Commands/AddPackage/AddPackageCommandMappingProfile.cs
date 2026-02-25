using AutoMapper;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Packages.Commands.AddPackage
{
    public class AddPackageCommandMappingProfile : Profile
    {
        public AddPackageCommandMappingProfile()
        {
            CreateMap<Package, AddPackageCommandResponse>()
                .ForMember(dest => dest.PackageId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
