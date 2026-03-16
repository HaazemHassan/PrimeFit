using AutoMapper;
using PrimeFit.Application.Features.Branches.Commands.CreateMemberWithSubscription;

namespace PrimeFit.API.Requests.Branches.Subscriptions
{
    public class CreateMemberWithSubscriptionRequestMappingProfile : Profile
    {
        public CreateMemberWithSubscriptionRequestMappingProfile()
        {
            CreateMap<CreateMemberWithSubscriptionRequest, CreateMemberWithSubscriptionCommand>();
        }
    }
}
