using AutoMapper;
using PrimeFit.Application.Features.Branches.Commands.CreateMemberWithSubscription;

namespace PrimeFit.Api.Requests.Subscriptions.CreateMemberWithSubscription {
    public class CreateMemberWithSubscriptionRequestMappingProfile : Profile
    {
        public CreateMemberWithSubscriptionRequestMappingProfile()
        {
            CreateMap<CreateMemberWithSubscriptionRequest, CreateMemberWithSubscriptionCommand>();
        }
    }
}
