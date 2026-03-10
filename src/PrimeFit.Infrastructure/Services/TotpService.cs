using OtpNet;
using PrimeFit.Application.ServicesContracts.Infrastructure;

namespace PrimeFit.Infrastructure.Services
{
    public class TotpService : ITotpService
    {
        public string GenerateTotpSecret()
        {
            var buffer = new byte[20];

            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(buffer);

            var QrSecretKey = Base32Encoding.ToString(buffer);

            return QrSecretKey;
        }

        public bool VerifyTotpCode(string secretKey, string code)
        {
            if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(code))
                return false;

            var secretBytes = Base32Encoding.ToBytes(secretKey);


            var totp = new Totp(secretBytes, step: 30, totpSize: 6);

            return totp.VerifyTotp(code, out _, new VerificationWindow(previous: 1, future: 1));
        }
    }
}
