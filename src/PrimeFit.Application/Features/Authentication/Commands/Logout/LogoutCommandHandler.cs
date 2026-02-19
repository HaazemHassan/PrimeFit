using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Infrastructure;
using PrimeFit.Domain.Contracts.Repositories;

namespace PrimeFit.Application.Features.Authentication.Commands.Logout;

public class LogoutCommandHandler(IAuthenticationService _authenticationService, IUnitOfWork _unitOfWork)
    : IRequestHandler<LogoutCommand, ErrorOr<Success>> {

    public async Task<ErrorOr<Success>> Handle(LogoutCommand request, CancellationToken cancellationToken) {
        var serviceResult = await _authenticationService.LogoutAsync(request.RefreshToken!);
        if (!serviceResult.IsError)
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        return serviceResult;
    }
}
