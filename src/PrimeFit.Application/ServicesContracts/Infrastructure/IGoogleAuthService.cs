using ErrorOr;
using PrimeFit.Application.Features.Authentication.Common;

namespace PrimeFit.Application.ServicesContracts.Infrastructure
{
    public interface IGoogleAuthService
    {
        Task<ErrorOr<GoogleUserInfo>> ValidateIdTokenAsync(string idToken, CancellationToken ct = default);

    }
}
