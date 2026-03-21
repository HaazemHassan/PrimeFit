using AutoMapper;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchById
{
    public class GetBranchByIdQueryMappingProfile : Profile
    {
        public GetBranchByIdQueryMappingProfile()
        {
            CreateMap<Branch, GetBranchByIdQueryResponse>()


            .ForMember(dest => dest.Images,
                opt => opt.MapFrom(src =>
                    src.Images
                        .Where(i => i.Status == BranchImageStatus.Active)
                        .OrderBy(i => i.DisplayOrder)
                ))


            .ForMember(dest => dest.ActivePackagesCount,
                opt => opt.MapFrom(src =>
                    src.Packages.Count(p => p.IsActive)
                ))


            .ForMember(dest => dest.ActiveSubscriptionsCount,
                opt => opt.MapFrom(src =>
                    src.Subscriptions.Count(s => s.Status == SubscriptionStatus.Active)
                ));
        }
    }
}
