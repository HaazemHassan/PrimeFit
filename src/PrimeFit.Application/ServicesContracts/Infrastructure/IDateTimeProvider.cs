namespace PrimeFit.Application.ServicesContracts.Infrastructure
{
    public interface IDateTimeProvider
    {
        DateTimeOffset UtcNow { get; }
        DateTimeOffset GetNow(string timeZoneId);
        DateTimeOffset ConvertToTimeZone(DateTimeOffset dateTime, string timeZoneId);
    }
}
