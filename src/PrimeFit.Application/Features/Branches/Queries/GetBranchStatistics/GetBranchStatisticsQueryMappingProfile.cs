//using AutoMapper;
//using PrimeFit.Domain.Common.Enums;
//using PrimeFit.Domain.Entities;

//namespace PrimeFit.Application.Features.Branches.Queries.GetBranchStatistics
//{
//    public class GetBranchStatisticsQueryMappingProfile : Profile
//    {
//        public GetBranchStatisticsQueryMappingProfile()
//        {
//            CreateMap<Branch, GetBranchStatisticsQueryResponse>()
//                .ForMember(dest => dest.NewSubscriptionsCount, opt => opt.MapFrom(src =>
//                    src.Subscriptions.Count(s => s.Status == SubscriptionStatus.Scheduled)))

//                .ForMember(dest => dest.ExpiredSubscriptionsCount, opt => opt.MapFrom(src =>
//                    src.Subscriptions.Count(s => s.Status == SubscriptionStatus.Expired)))

//                .ForMember(dest => dest.TotalRevenue, opt => opt.MapFrom(src =>
//                    src.Subscriptions.Sum(s => s.PaidAmount)));
//        }
//    }
//}
