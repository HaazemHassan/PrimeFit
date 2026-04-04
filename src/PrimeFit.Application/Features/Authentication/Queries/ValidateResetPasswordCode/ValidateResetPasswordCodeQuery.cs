using ErrorOr;
using MediatR;

namespace PrimeFit.Application.Features.Authentication.Queries.ValidateResetPasswordCode
{
    public class ValidateResetPasswordCodeQuery : IRequest<ErrorOr<ValidateResetPasswordCodeQueryResponse>>
    {
        public string Email { get; set; } = null!;
        public string Code { get; set; } = null!;
    }
}
