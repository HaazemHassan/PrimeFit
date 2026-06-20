using FluentValidation;

namespace PrimeFit.Application.Features.Payments.Commands.InitializeSubscriptionPayment
{
    public class InitializeSubscriptionPaymentCommandValidator : AbstractValidator<InitializeSubscriptionPaymentCommand>
    {
        public InitializeSubscriptionPaymentCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID is required.");

            RuleFor(x => x.BranchId)
                .GreaterThan(0).WithMessage("Branch ID is required.");

            RuleFor(x => x.PackageId)
                .GreaterThan(0).WithMessage("Package ID is required.");
        }
    }
}
