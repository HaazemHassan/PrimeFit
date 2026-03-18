namespace PrimeFit.Infrastructure.Common.Options
{
    public class MailSettings
    {
        public const string SectionName = "MailSettings";

        public string DisplayName { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
    }
}
