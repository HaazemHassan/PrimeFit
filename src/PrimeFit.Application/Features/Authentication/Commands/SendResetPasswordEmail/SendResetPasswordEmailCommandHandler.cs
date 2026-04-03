using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Contracts.Infrastructure;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Authentication.Commands.SendResetPasswordEmail
{
    internal class SendResetPasswordEmailCommandHandler : IRequestHandler<SendResetPasswordEmailCommand, ErrorOr<Success>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IPasswordService _passwordService;
        private readonly IUnitOfWork _unitOfWork;

        public SendResetPasswordEmailCommandHandler(ICurrentUserService currentUserService, IPasswordService passwordService, IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _passwordService = passwordService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Success>> Handle(SendResetPasswordEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetAsync(u => u.Email == request.Email, cancellationToken);
            if (user is null)
            {
                return Result.Success;
            }

            var result = await _passwordService.CreatePasswordResetCode(user.Id, cancellationToken);

            if (result.IsError)
            {
                return result.Errors;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _passwordService.SendPasswordResetEmailAsync(user, result.Value);

            return Result.Success;
        }
    }
}
