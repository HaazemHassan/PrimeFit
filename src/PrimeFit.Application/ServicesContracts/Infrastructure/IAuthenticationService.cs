using ErrorOr;
using PrimeFit.Application.Features.Authentication.Common;

namespace PrimeFit.Application.ServicesContracts.Infrastructure
{
    public interface IAuthenticationService
    {
        public Task<ErrorOr<AuthResult>> SignInWithPasswordAsync(string Email, string password, CancellationToken ct = default);
        public Task<ErrorOr<AuthResult>> ReAuthenticateAsync(string refreshToken, string accessToken, CancellationToken ct = default);
        public Task<ErrorOr<Success>> LogoutAsync(string refreshToken, CancellationToken ct = default);
        Task<ErrorOr<AuthResult>> SignInWithGoogleAsync(GoogleUserInfo googleUser, CancellationToken ct = default);
    }
}

