using AutoMapper;
using PrimeFit.API.Requests.Branches;
using PrimeFit.API.Requests.Branches.Subscriptions;
using PrimeFit.API.Requests.BranchReviews;
using PrimeFit.API.Requests.Client.Users;
using PrimeFit.API.Requests.Subscriptions;
using PrimeFit.Application.Features.Branches.Commands.CreateMemberWithSubscription;
using PrimeFit.Application.Features.Branches.Commands.UpdateBasicDetails;
using PrimeFit.Application.Features.Branches.Commands.UpdateLocationDetails;
using PrimeFit.Application.Features.Branches.Commands.UpdateWorkingHours;
using PrimeFit.Application.Features.Branches.Queries.GetBranchSetupDetails;
using PrimeFit.Application.Features.BranchPackages.Commands.AddPackage;
using PrimeFit.Application.Features.BranchPackages.Commands.UpdatePackage;
using PrimeFit.Application.Features.BranchPackages.Queries.GetBranchPackages;
using PrimeFit.Application.Features.BranchReviews.Queries.GetBranchReviews;
using PrimeFit.Application.Features.Subscriptions.Commands.AddSubscription;
using PrimeFit.Application.Features.Subscriptions.Queries.GetBranchSubscriptions;
using PrimeFit.Application.Features.Subscriptions.Queries.GetSubscriptionAttendanceHistory;
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
            CreateMap<GetBranchSetupDetailsRequest, GetBranchSetupDetailsQuery>();
            CreateMap<GetBranchSubscriptionsRequest, GetBranchSubscriptionsQuery>().ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember is not null));

            CreateMap<GetSubscriptionAttendanceHistoryRequest, GetSubscriptionAttendanceHistoryQuery>();

            CreateMap<CreateMemberWithSubscriptionRequest, CreateMemberWithSubscriptionCommand>();
            CreateMap<AddSubscriptionRequest, AddSubscriptionCommand>();


            CreateMap<BasicPaginationRequest, GetBranchPackagesQuery>().ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember is not null));

            CreateMap<GetBranchReviewsRequest, GetBranchReviewsQuery>().ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember is not null));
        }

    }
}
