using ErrorOr;
using MediatR;
using PrimeFit.Application.Contracts.Infrastructure;

namespace PrimeFit.Application.Features.Authentication.Queries.ValidateResetPasswordCode
{
    internal class ValidateResetPasswordCodeQueryHandler : IRequestHandler<ValidateResetPasswordCodeQuery, ErrorOr<ValidateResetPasswordCodeQueryResponse>>
    {
        private readonly IPasswordService _passwordService;

        public ValidateResetPasswordCodeQueryHandler(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        public async Task<ErrorOr<ValidateResetPasswordCodeQueryResponse>> Handle(ValidateResetPasswordCodeQuery request, CancellationToken cancellationToken)
        {
            var isValidResult = await _passwordService.IsPasswordResetCodeValid(request.Email, request.Code, cancellationToken);

            if (isValidResult.IsError)
            {
                return isValidResult.Errors;
            }

            return new ValidateResetPasswordCodeQueryResponse
            {
                IsValid = isValidResult.Value
            };
        }
    }
}
