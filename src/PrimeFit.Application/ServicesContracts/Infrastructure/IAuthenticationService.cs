using ErrorOr;
using PrimeFit.Application.Features.Authentication.Common;
using PrimeFit.Domain.Entities;

namespace PrimeFit.Application.Contracts.Infrastructure
{
    public interface IAuthenticationService
    {
        public Task<ErrorOr<AuthResult>> SignInWithPassword(string Email, string password, CancellationToken ct = default);
        public Task<ErrorOr<AuthResult>> ReAuthenticateAsync(string refreshToken, string accessToken, CancellationToken ct = default);
        public Task<ErrorOr<Success>> LogoutAsync(string refreshToken, CancellationToken ct = default);
        public Task<ErrorOr<Success>> ChangePassword(int domainUserId, string currentPassword, string newPassword);
        public Task<ErrorOr<Success>> ConfirmEmail(int appUserId, string code, CancellationToken ct = default);
        public Task<ErrorOr<string>> CreateEmailConfirmationCode(int domainUserId, CancellationToken ct = default);
        public Task SendConfirmationEmailAsync(DomainUser user, string code);

    }
}
