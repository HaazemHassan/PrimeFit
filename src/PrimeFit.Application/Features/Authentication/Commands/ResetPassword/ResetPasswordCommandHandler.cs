using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Infrastructure;
using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Application.Features.Authentication.Commands.ResetPassword
{
    internal class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ErrorOr<Success>>
    {
        private readonly IPasswordService _passwordService;
        private readonly IUnitOfWork _unitOfWork;

        public ResetPasswordCommandHandler(IPasswordService passwordService, IUnitOfWork unitOfWork)
        {
            _passwordService = passwordService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Success>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await _passwordService.ResetPassword(request.Email, request.Code, request.NewPassword, cancellationToken);

            if (result.IsError)
            {
                return result.Errors;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success;
        }
    }
}
