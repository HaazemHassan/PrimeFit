using AutoMapper;
using PrimeFit.Application.Features.Users.Common;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Users.Mapping;

public partial class UserResponseMappingProfile : Profile
{
    public UserResponseMappingProfile()
    {
        CreateMap<DomainUser, BaseUserResponse>()
            .ForMember(dest => dest.FullName,
               opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
    }
}
