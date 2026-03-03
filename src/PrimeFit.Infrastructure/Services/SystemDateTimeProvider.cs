using PrimeFit.Application.ServicesContracts.Infrastructure;

namespace PrimeFit.Infrastructure.Services
{
    internal class SystemDateTimeProvider : IDateTimeProvider
    {
        private readonly TimeProvider _timeProvider;

        public SystemDateTimeProvider(TimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public DateTimeOffset UtcNow => _timeProvider.GetUtcNow();

        public DateTimeOffset ConvertToTimeZone(DateTimeOffset dateTime, string timeZoneId)
        {
            if (string.IsNullOrWhiteSpace(timeZoneId))
                throw new ArgumentException("Time zone id is required.", nameof(timeZoneId));

            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

            return TimeZoneInfo.ConvertTime(dateTime, timeZone);
        }

        public DateTimeOffset GetNow(string timeZoneId)
        {
            if (string.IsNullOrWhiteSpace(timeZoneId))
                throw new ArgumentException("Time zone id is required.", nameof(timeZoneId));

            var utcNow = _timeProvider.GetUtcNow();

            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

            return TimeZoneInfo.ConvertTime(utcNow, timeZone);
        }
    }
}
