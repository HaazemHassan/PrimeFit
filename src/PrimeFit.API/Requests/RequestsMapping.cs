using AutoMapper;
using PrimeFit.API.Requests.Client.Users;
using PrimeFit.API.Requests.Owner.Branches;
using PrimeFit.Application.Features.Branches.Commands.AddWorkingHours;
using PrimeFit.Application.Features.Branches.Commands.UpdateBasicDetails;
using PrimeFit.Application.Features.Branches.Commands.UpdateLocationDetails;
using PrimeFit.Application.Features.BranchPackages.Queries.GetBranchPackagesForOwner;
using PrimeFit.Application.Features.Packages.Commands.AddPackage;
using PrimeFit.Application.Features.Packages.Commands.UpdatePackage;
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
            CreateMap<UpdateBussinessDetailsRequest, UpdateBussinessDetailsCommand>();


            CreateMap<BasicPaginationRequest, GetBranchPackagesForOwnerQuery>().ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember is not null));



        }

    }
}
