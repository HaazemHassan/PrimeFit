using FluentValidation;

namespace PrimeFit.Application.Features.BranchReviews.Commands.UpdateMyBranchReview
{
    public class UpdateMyBranchReviewCommandValidator : AbstractValidator<UpdateMyBranchReviewCommand>
    {
        public UpdateMyBranchReviewCommandValidator()
        {
            RuleFor(x => x.ReviewId)
                .GreaterThan(0);

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5);

            RuleFor(x => x.Comment)
                .MaximumLength(300)
                .When(x => x.Comment is not null);
        }
    }
}
