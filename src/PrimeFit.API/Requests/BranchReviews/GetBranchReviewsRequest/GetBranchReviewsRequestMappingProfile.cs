using AutoMapper;
using PrimeFit.Application.Features.BranchReviews.Queries.GetBranchReviews;

namespace PrimeFit.API.Requests.BranchReviews
{
    public class GetBranchReviewsRequestMappingProfile : Profile
    {
        public GetBranchReviewsRequestMappingProfile()
        {
            CreateMap<GetBranchReviewsRequest, GetBranchReviewsQuery>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember is not null));
        }
    }
}
