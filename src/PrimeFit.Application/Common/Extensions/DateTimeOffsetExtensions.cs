namespace PrimeFit.Application.Common.Extensions
{
    public static class DateTimeOffsetExtensions
    {
        public static DateTimeOffset StartOfMonth(this DateTimeOffset date)
        {
            return new DateTimeOffset(date.Year, date.Month, 1, 0, 0, 0, date.Offset);
        }
    }
}
