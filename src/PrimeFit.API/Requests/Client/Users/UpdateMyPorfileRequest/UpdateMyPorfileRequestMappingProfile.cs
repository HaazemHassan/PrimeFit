using AutoMapper;
using PrimeFit.Application.Features.Users.Commands.UpdateProfile;

namespace PrimeFit.API.Requests.Client.Users
{
    public class UpdateMyPorfileRequestMappingProfile : Profile
    {
        public UpdateMyPorfileRequestMappingProfile()
        {
            CreateMap<UpdateMyPorfileRequest, UpdateProfileCommand>();
        }
    }
}
