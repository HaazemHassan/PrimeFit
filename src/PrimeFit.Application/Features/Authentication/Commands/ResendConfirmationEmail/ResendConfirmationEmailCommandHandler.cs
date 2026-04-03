using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Contracts.Infrastructure;
using PrimeFit.Application.Features.Authentication.Commands.ResendConfirmEmail;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Authentication.Commands.ResendConfirmationEmail
{
    internal class ResendConfirmationEmailCommandHandler : IRequestHandler<ResendConfirmationEmailCommand, ErrorOr<Success>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IEmailVerificationService _emailVerificationService;
        private readonly IUnitOfWork _unitOfWork;

        public ResendConfirmationEmailCommandHandler(ICurrentUserService currentUserService, IEmailVerificationService emailVerificationService, IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _emailVerificationService = emailVerificationService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Success>> Handle(ResendConfirmationEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetAsync(u => u.Email == request.Email, cancellationToken);
            if (user is null)
            {
                return Result.Success;
            }

            var result = await _emailVerificationService.CreateEmailConfirmationCode(user.Id, cancellationToken);

            if (result.IsError)
            {
                return result.Errors;
            }

            await _emailVerificationService.SendConfirmationEmailAsync(user, result.Value);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success;
        }
    }
}
