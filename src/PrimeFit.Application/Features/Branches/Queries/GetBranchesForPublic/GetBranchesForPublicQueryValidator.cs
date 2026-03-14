using FluentValidation;

namespace PrimeFit.Application.Features.Branches.Queries.GetBranchesForPublic
{
    public class GetBranchesForPublicQueryValidator : AbstractValidator<GetBranchesForPublicQuery>
    {
        public GetBranchesForPublicQueryValidator()
        {


            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            When(x => x.Latitude.HasValue, () =>
            {
                RuleFor(x => x.Latitude)
               .InclusiveBetween(-90, 90);
            });


            When(x => x.Longitude.HasValue, () =>
            {
                RuleFor(x => x.Longitude)
               .InclusiveBetween(-180, 180);
            });


            RuleFor(x => x.Search)
              .MaximumLength(100);


        }

    }
}
