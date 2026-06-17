namespace PrimeFit.Application.Common.DTOS
{
    public class PushNotificationRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public Dictionary<string, string>? Data { get; set; }
        public string? ImageUrl { get; set; }
    }
}
