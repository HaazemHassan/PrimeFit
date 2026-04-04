using ErrorOr;
using MediatR;
using PrimeFit.Application.Features.Authentication.Common;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Authentication.Commands.SignIn;

public class SignInCommandHandler(IAuthenticationService _authenticationService, IUnitOfWork _unitOfWork)
    : IRequestHandler<SignInWithPasswordCommand, ErrorOr<AuthResult>>
{

    public async Task<ErrorOr<AuthResult>> Handle(SignInWithPasswordCommand request, CancellationToken cancellationToken)
    {
        var authResult = await _authenticationService.SignInWithPasswordAsync(request.Email, request.Password, cancellationToken);

        if (!authResult.IsError)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);

        }
        return authResult;
    }
}
