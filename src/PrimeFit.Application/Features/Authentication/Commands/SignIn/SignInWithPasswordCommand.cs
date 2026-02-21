using ErrorOr;
using MediatR;
using PrimeFit.Application.Features.Authentication.Common;

namespace PrimeFit.Application.Features.Authentication.Commands.SignIn;

public class SignInWithPasswordCommand : IRequest<ErrorOr<AuthResult>>
{
    public string Email { get; set; }
    public string Password { get; set; }

}
