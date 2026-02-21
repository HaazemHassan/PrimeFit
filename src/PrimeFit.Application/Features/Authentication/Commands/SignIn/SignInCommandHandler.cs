using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Infrastructure;
using PrimeFit.Application.Features.Authentication.Common;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Application.Features.Authentication.Commands.SignIn;

public class SignInCommandHandler(IAuthenticationService _authenticationService, IUnitOfWork _unitOfWork)
    : IRequestHandler<SignInWithPasswordCommand, ErrorOr<AuthResult>> {

    public async Task<ErrorOr<AuthResult>> Handle(SignInWithPasswordCommand request, CancellationToken cancellationToken) {
        var authResult = await _authenticationService.SignInWithPassword(request.Email, request.Password);
        if (!authResult.IsError)
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        return authResult;
    }
}
