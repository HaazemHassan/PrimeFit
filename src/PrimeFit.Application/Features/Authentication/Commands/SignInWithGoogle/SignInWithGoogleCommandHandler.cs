using ErrorOr;
using MediatR;
using PrimeFit.Application.Features.Authentication.Common;
using PrimeFit.Application.ServicesContracts.Infrastructure;

namespace PrimeFit.Application.Features.Authentication.Commands.SignInWithGoogle
{
    internal sealed class SignInWithGoogleCommandHandler(
     IGoogleAuthService googleAuthService,
     IAuthenticationService authenticationService) : IRequestHandler<SignInWithGoogleCommand, ErrorOr<AuthResult>>
    {
        public async Task<ErrorOr<AuthResult>> Handle(SignInWithGoogleCommand request, CancellationToken cancellationToken)
        {
            var googleUserResult = await googleAuthService.ValidateIdTokenAsync(request.IdToken, cancellationToken);
            if (googleUserResult.IsError)
            {
                return googleUserResult.Errors;

            }

            return await authenticationService.SignInWithGoogleAsync(googleUserResult.Value, cancellationToken);
        }
    }
}
