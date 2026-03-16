using FluentValidation;

namespace PrimeFit.Application.Features.Subscriptions.Queries.GetSubscriptionAttendanceHistory
{
    public class GetSubscriptionAttendanceHistoryQueryValidator : AbstractValidator<GetSubscriptionAttendanceHistoryQuery>
    {
        public GetSubscriptionAttendanceHistoryQueryValidator()
        {
            When(x => x.Year.HasValue, () =>
            {
                RuleFor(x => x.Year)
                    .InclusiveBetween(1, 9999);
            });

            When(x => x.Month.HasValue, () =>
            {
                RuleFor(x => x.Month)
                    .InclusiveBetween(1, 12);
            });
        }
    }
}