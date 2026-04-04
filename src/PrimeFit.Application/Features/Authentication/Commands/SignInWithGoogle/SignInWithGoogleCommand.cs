using ErrorOr;
using MediatR;
using PrimeFit.Application.Features.Authentication.Common;

namespace PrimeFit.Application.Features.Authentication.Commands.SignInWithGoogle
{
    public class SignInWithGoogleCommand : IRequest<ErrorOr<AuthResult>>
    {
        public string IdToken { get; set; }
    }
}
