using FluentValidation;

namespace PrimeFit.Application.Features.Authentication.Commands.SignInWithGoogle
{
    public class SignInWithGoogleCommandValidator : AbstractValidator<SignInWithGoogleCommand>
    {
        public SignInWithGoogleCommandValidator()
        {
            RuleFor(x => x.IdToken)
                .NotEmpty().WithMessage("Google ID token is required.")
                .MinimumLength(100).WithMessage("Invalid Google ID token format.");
        }
    }

}
