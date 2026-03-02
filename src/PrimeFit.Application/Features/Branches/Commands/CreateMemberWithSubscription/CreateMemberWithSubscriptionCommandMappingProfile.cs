using AutoMapper;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Branches.Commands.CreateMemberWithSubscription
{
    public class CreateMemberWithSubscriptionCommandMappingProfile : Profile
    {
        public CreateMemberWithSubscriptionCommandMappingProfile()
        {
            CreateMap<Subscription, CreateMemberWithSubscriptionCommandResponse>();
        }
    }
}

