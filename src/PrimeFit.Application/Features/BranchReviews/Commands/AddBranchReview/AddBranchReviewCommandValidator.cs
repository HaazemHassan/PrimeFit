using FluentValidation;

namespace PrimeFit.Application.Features.BranchReviews.Commands.AddBranchReview
{
    public class AddBranchReviewCommandValidator : AbstractValidator<AddBranchReviewCommand>
    {
        public AddBranchReviewCommandValidator()
        {
            RuleFor(x => x.BranchId)
                .GreaterThan(0);

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5);

            RuleFor(x => x.Comment)
                .MaximumLength(300)
                .When(x => x.Comment is not null);
        }
    }
}
