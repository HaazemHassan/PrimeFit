using AutoMapper;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Users.Commands.UpdateProfile
{
    public class UpdateProfileCommandMappingProfile : Profile
    {
        public UpdateProfileCommandMappingProfile()
        {
            CreateMap<DomainUser, UpdateProfileCommandResponse>();
        }

    }
}
