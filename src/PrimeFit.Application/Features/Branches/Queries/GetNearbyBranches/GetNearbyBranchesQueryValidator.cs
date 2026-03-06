using FluentValidation;

namespace PrimeFit.Application.Features.Branches.Queries.GetNearbyBranches
{
    public class GetNearbyBranchesQueryValidator : AbstractValidator<GetNearbyBranchesQuery>
    {
        public GetNearbyBranchesQueryValidator()
        {


            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90);

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180);

            RuleFor(x => x.RadiusInMeters)
                .GreaterThan(0)
                .LessThanOrEqualTo(15000);



            RuleFor(x => x.Search)
              .MaximumLength(100);




        }

    }
}
