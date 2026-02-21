using ErrorOr;
using MediatR;
using PrimeFit.Application.Common.Behaviors.Transaction;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Markers;
using PrimeFit.Domain.Common.Enums;

namespace PrimeFit.Application.Features.Users.Commands.AddUser
{
    [Authorize(Permissions = [Permission.UsersWrite])]
    public class AddUserCommand : IRequest<ErrorOr<AddUserCommandResponse>>, ITransactionalRequest, IAuthorizedRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public UserRole UserRole { get; set; } = UserRole.Member;

    }
}
