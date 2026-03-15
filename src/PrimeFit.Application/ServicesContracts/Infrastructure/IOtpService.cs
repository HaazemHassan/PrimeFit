namespace PrimeFit.Application.ServicesContracts.Infrastructure
{
    public interface IOtpService
    {
        public string Generate(int length = 6);

    }
}
