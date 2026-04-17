using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Infrastructure;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Authentication.Commands.ConfirmEmail
{
    internal class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, ErrorOr<Success>>
    {
        private readonly IEmailVerificationService _emailVerificationService;
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmEmailCommandHandler(IEmailVerificationService emailVerificationService, IUnitOfWork unitOfWork)
        {
            _emailVerificationService = emailVerificationService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Success>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetAsync(u => u.Email == request.Email, cancellationToken);
            if (user is null)
            {
                return Error.NotFound(code: ErrorCodes.User.UserNotFound, description: "User not found.");
            }

            var result = await _emailVerificationService.ConfirmEmail(user.Id, request.Code, cancellationToken);

            if (result.IsError)
            {
                return result.Errors;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
