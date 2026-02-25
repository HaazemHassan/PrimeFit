using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Behaviors.Transaction;
using PrimeFit.Application.Features.Users.Common;

namespace PrimeFit.Application.Features.Authentication.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<ErrorOr<BaseUserResponse>>, ITransactionalRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

    }
}
