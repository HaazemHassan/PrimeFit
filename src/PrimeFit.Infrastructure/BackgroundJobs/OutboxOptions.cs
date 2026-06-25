namespace PrimeFit.Infrastructure.BackgroundJobs
{
    public class OutboxOptions
    {
        public const string SectionName = "OutboxSettings";
        public int ProcessingIntervalSeconds { get; set; } = 30;
        public int BatchSize { get; set; } = 100;
    }
}
