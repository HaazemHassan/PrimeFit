using AutoMapper;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Features.BranchReviews.Queries.GetBranchReviews
{
    public class GetBranchReviewsQueryMappingProfile : Profile
    {
        public GetBranchReviewsQueryMappingProfile()
        {
            CreateMap<BranchReview, GetBranchReviewsQueryResponse>()
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
                .ForMember(dest => dest.ReviewedAt, opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}
