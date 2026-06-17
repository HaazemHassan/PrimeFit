using FluentValidation;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Notifications.Commands.RegisterDeviceToken
{
    public class RegisterDeviceTokenCommandValidator : AbstractValidator<RegisterDeviceTokenCommand>
    {
        public RegisterDeviceTokenCommandValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("FCM token is required.")
                .MaximumLength(500).WithMessage("FCM token must not exceed 500 characters.");

            RuleFor(x => x.DevicePlatform)
                .IsInEnum().WithMessage("Invalid device platform.");
        }
    }
}
