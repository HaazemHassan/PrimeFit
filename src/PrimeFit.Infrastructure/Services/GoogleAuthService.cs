using ErrorOr;
using Google.Apis.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PrimeFit.Application.Features.Authentication.Common;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.Common.Constants;
using PrimeFit.Infrastructure.Common.Options;

namespace PrimeFit.Infrastructure.Services
{
    internal sealed class GoogleAuthService(IOptions<GoogleAuthOptions> googleOptions, ILogger<GoogleAuthService> logger) : IGoogleAuthService
    {
        private readonly GoogleAuthOptions _googleOptions = googleOptions.Value;

        public async Task<ErrorOr<GoogleUserInfo>> ValidateIdTokenAsync(string idToken, CancellationToken ct = default)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = [_googleOptions.ClientId],
                    ExpirationTimeClockTolerance = TimeSpan.FromMinutes(1),
                    IssuedAtClockTolerance = TimeSpan.FromMinutes(1)
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

                if (!payload.EmailVerified)
                {
                    logger.LogWarning(
                        "Google login attempt with unverified email: {Email}",
                        payload.Email);

                    return Error.Validation(
                        code: ErrorCodes.Authentication.InvalidCredentials,
                        description: "Google account email is not verified.");
                }

                var firstName =
                    payload.GivenName ??
                    payload.Name?.Split(' ').FirstOrDefault() ??
                    "User";

                var lastName =
                    payload.FamilyName ??
                    payload.Name?.Split(' ').LastOrDefault() ??
                    string.Empty;

                return new GoogleUserInfo(
                    payload.Subject,
                    payload.Email,
                    firstName,
                    lastName,
                    payload.EmailVerified
                );
            }
            catch (InvalidJwtException ex)
            {
                logger.LogWarning(
                    ex,
                    "Invalid or expired Google ID token received");

                return Error.Validation(
                    code: ErrorCodes.Authentication.InvalidAccessToken,
                    description: "Invalid ID token.");
            }
        }
    }
}


