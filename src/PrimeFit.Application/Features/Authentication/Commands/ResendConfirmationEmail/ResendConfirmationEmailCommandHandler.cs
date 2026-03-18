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
        private readonly IAuthenticationService _authenticationService;
        private readonly IUnitOfWork _unitOfWork;

        public ResendConfirmationEmailCommandHandler(ICurrentUserService currentUserService, IAuthenticationService authenticationService, IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _authenticationService = authenticationService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Success>> Handle(ResendConfirmationEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetAsync(u => u.Email == request.Email, cancellationToken);
            if (user is null)
            {
                return Result.Success;
            }

            var result = await _authenticationService.CreateEmailConfirmationCode(user.Id, cancellationToken);

            if (result.IsError)
            {
                return result.Errors;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success;
        }
    }
}
