using AutoMapper;
using PrimeFit.API.Requests.Client.Users;
using PrimeFit.API.Requests.Owner.Branches;
using PrimeFit.API.Requests.Owner.Branches.AddPackage;
using PrimeFit.API.Requests.Owner.Branches.UpdatePackage;
using PrimeFit.Application.Features.Branches.Commands.AddPackage;
using PrimeFit.Application.Features.Branches.Commands.AddWorkingHours;
using PrimeFit.Application.Features.Branches.Commands.UpdateLocationDetails;
using PrimeFit.Application.Features.Branches.Commands.UpdatePackage;
using PrimeFit.Application.Features.Users.Commands.UpdateProfile;

namespace PrimeFit.API.Requests
{
    public class RequestsMappingProfile : Profile
    {
        public RequestsMappingProfile()
        {

            //Users requests
            CreateMap<UpdateMyPorfileRequest, UpdateProfileCommand>();


            //Owner requests
            CreateMap<UpdateLocationDetailsRequest, UpdateLocationDetailsCommand>();
            CreateMap<UpdateWorkingHoursRequest, UpdateWorkingHoursCommand>();
            CreateMap<AddPackageRequest, AddPackageCommand>();
            CreateMap<UpdatePackageRequest, UpdatePackageCommand>();


        }

    }
}
