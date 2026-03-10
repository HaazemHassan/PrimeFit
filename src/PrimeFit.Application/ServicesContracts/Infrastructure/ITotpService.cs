namespace PrimeFit.Application.ServicesContracts.Infrastructure
{
    public interface ITotpService
    {
        public string GenerateTotpSecret();
        bool VerifyTotpCode(string secretKey, string code);
    }
}
