using AutoMapper;
using PrimeFit.Application.Features.BranchReviews.Commands.AddBranchReview;

namespace PrimeFit.Api.Requests.BranchReviews.AddBranchReview;

public class AddBranchReviewRequestMappingProfile : Profile
{
    public AddBranchReviewRequestMappingProfile()
    {
        CreateMap<AddBranchReviewRequest, AddBranchReviewCommand>();
    }
}
