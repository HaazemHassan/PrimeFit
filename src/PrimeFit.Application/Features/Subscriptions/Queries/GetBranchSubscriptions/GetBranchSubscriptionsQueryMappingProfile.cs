//using AutoMapper;
//using PrimeFit.Application.Features.Packages.Commands.AddPackage;
//using PrimeFit.Domain.Entities;

//namespace PrimeFit.Application.Features.Subscriptions.Queries.GetBranchSubscriptions
//{
//    public class GetBranchSubscriptionsQueryMappingProfile : Profile
//    {

//        public GetBranchSubscriptionsQueryMappingProfile()
//        {

//            CreateMap<Subscription, GetBranchSubscriptionsQueryResponse>()
//               .ForMember(dest => dest.SubscriptionId, opt => opt.MapFrom(src => src.Id))
//               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
//               .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
//               .ForMember(dest => dest.TotalDurationInDays, opt => opt.MapFrom(src => src.GetRemainingDays());

//        }
//    }
//}
