namespace PrimeFit.Application.ServicesContracts.Infrastructure
{
    public interface IEmailBodyBuilderService
    {
        public string GenerateEmailBody(string templateName, Dictionary<string, string> templateModel);

    }
}
