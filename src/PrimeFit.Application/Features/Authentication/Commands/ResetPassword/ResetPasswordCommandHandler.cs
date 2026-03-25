using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.Contracts.Infrastructure;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Authentication.Commands.ResetPassword
{
    internal class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ErrorOr<Success>>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUnitOfWork _unitOfWork;

        public ResetPasswordCommandHandler(IAuthenticationService authenticationService, IUnitOfWork unitOfWork)
        {
            _authenticationService = authenticationService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Success>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.ResetPassword(request.Email, request.code, request.NewPassword, cancellationToken);

            if (result.IsError)
            {
                return result.Errors;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success;
        }
    }
}
