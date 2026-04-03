using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Contracts.Infrastructure;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Authentication.Commands.ConfirmEmail
{
    internal class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, ErrorOr<Success>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IEmailVerificationService _emailVerificationService;
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmEmailCommandHandler(ICurrentUserService currentUserService, IEmailVerificationService emailVerificationService, IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _emailVerificationService = emailVerificationService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Success>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var result = await _emailVerificationService.ConfirmEmail(userId!.Value, request.code, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
