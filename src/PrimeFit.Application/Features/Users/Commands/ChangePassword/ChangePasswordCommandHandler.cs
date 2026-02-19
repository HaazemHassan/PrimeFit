using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Contracts.Infrastructure;

namespace PrimeFit.Application.Features.Users.Commands.ChangePassword {

    public class ChangePasswordCommandHandler(IAuthenticationService _authenticationService, ICurrentUserService _currentUserService)
        : IRequestHandler<ChangePasswordCommand, ErrorOr<Success>> {

        public async Task<ErrorOr<Success>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken) {
            var userId = _currentUserService.UserId;
            return await _authenticationService.ChangePassword(userId!.Value, request.CurrentPassword, request.NewPassword);
        }
    }
}
