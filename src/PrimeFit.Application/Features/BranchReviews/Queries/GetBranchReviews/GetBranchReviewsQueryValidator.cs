using FluentValidation;

namespace PrimeFit.Application.Features.BranchReviews.Queries.GetBranchReviews
{
    public class GetBranchReviewsQueryValidator : AbstractValidator<GetBranchReviewsQuery>
    {
        public GetBranchReviewsQueryValidator()
        {
            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5)
                .When(x => x.Rating.HasValue);
        }
    }
}
