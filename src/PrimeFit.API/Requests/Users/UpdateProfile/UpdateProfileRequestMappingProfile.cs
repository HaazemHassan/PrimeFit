using AutoMapper;
using PrimeFit.Application.Features.Users.Commands.UpdateProfile;

namespace PrimeFit.Api.Requests.Users.UpdateProfile {
    public class UpdateProfileRequestMappingProfile : Profile
    {
        public UpdateProfileRequestMappingProfile()
        {
            CreateMap<UpdateProfileRequest, UpdateProfileCommand>();
        }
    }
}
