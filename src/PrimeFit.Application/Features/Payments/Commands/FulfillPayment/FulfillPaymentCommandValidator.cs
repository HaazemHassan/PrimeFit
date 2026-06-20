using FluentValidation;

namespace PrimeFit.Application.Features.Payments.Commands.FulfillPayment
{
    public class FulfillPaymentCommandValidator : AbstractValidator<FulfillPaymentCommand>
    {
        public FulfillPaymentCommandValidator()
        {
            RuleFor(x => x.StripePaymentIntentId)
                .NotEmpty();

            RuleFor(x => x.RequestId)
                .NotEmpty();
        }
    }
}
