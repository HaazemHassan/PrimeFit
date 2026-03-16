namespace PrimeFit.Application.Features.Subscriptions.Queries.GetSubscriptionAttendanceHistory
{
    public class GetSubscriptionAttendanceHistoryResponse
    {
        public int SubscriptionId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public List<int> AttendedDays { get; set; } = new();
        public int TotalAttendance { get; set; }
        public bool HasPreviousMonth { get; set; }
        public bool HasNextMonth { get; set; }
    }
}