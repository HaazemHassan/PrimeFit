namespace PrimeFit.Application.ServicesContracts.Infrastructure
{
    public interface IDateTimeProvider
    {
        DateTimeOffset UtcNow { get; }
        DateTimeOffset GetTimeZoneNow(string timeZoneId = "Africa/Cairo");
        DateTimeOffset ConvertToTimeZone(DateTimeOffset dateTime, string timeZoneId = "Africa/Cairo");
    }
}
