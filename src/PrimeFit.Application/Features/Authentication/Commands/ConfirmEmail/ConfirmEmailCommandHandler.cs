using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Contracts.Infrastructure;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Application.Features.Authentication.Commands.ConfirmEmail
{
    internal class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, ErrorOr<Success>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmEmailCommandHandler(ICurrentUserService currentUserService, IAuthenticationService authenticationService, IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _authenticationService = authenticationService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Success>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var result = await _authenticationService.ConfirmEmail(userId!.Value, request.code, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
