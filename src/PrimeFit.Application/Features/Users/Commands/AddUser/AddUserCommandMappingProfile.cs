using AutoMapper;
using PrimeFit.Application.Features.Users.Common;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Users.Commands.AddUser
{
    public class AddUserCommandMappingProfile : Profile
    {
        public AddUserCommandMappingProfile()
        {
            CreateMap<DomainUser, AddUserCommandResponse>()
                .IncludeBase<DomainUser, UserResponse>();
        }

    }
}
