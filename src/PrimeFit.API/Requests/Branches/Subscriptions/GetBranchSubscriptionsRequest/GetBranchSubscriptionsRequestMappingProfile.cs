using AutoMapper;
using PrimeFit.Application.Features.Subscriptions.Queries.GetBranchSubscriptions;

namespace PrimeFit.API.Requests.Branches.Subscriptions
{
    public class GetBranchSubscriptionsRequestMappingProfile : Profile
    {
        public GetBranchSubscriptionsRequestMappingProfile()
        {
            CreateMap<GetBranchSubscriptionsRequest, GetBranchSubscriptionsQuery>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember is not null));
        }
    }
}
