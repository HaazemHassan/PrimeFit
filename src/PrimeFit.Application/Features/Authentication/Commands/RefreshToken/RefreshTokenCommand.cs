using ErrorOr;
using MediatR;
using PrimeFit.Application.Features.Authentication.Common;

namespace PrimeFit.Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<ErrorOr<AuthResult>>
{
    public string AccessToken { get; set; } = string.Empty;
    public string? RefreshToken { get; set; }

}
