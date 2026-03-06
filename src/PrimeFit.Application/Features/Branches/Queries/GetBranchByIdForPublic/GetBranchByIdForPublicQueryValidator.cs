using FluentValidation;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchByIdForPublic
{
    public class GetBranchByIdForPublicQueryValidator : AbstractValidator<GetBranchByIdForPublicQuery>
    {
        public GetBranchByIdForPublicQueryValidator()
        {


            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90);

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180);

        }

    }
}
