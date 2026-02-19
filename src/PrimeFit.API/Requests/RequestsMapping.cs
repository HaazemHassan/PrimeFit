using AutoMapper;
using PrimeFit.API.Requests.Client.Users;
using PrimeFit.API.Requests.Management.Users;
using PrimeFit.Application.Features.Users.Commands.UpdateProfile;

namespace PrimeFit.API.Requests
{
    public class RequestsMappingProfile : Profile
    {
        public RequestsMappingProfile()
        {

            //Users requests
            CreateMap<UpdateMyPorfileRequest, UpdateProfileCommand>();


            //Admin requests
            CreateMap<UpdateUserRequest, UpdateProfileCommand>();
        }

    }
}
