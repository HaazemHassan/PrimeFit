namespace PrimeFit.API.RateLimiting {
    public class RateLimitingOptions {
        public const string SectionName = "RateLimitingSettings";

        public DefaultLimiterOptions DefaultLimiter { get; set; } = new();
        public LoginLimiterOptions LoginLimiter { get; set; } = new();
        public int RetryAfterSeconds { get; set; }
    }

    public class DefaultLimiterOptions {
        public int WindowMinutes { get; set; }
        public int PermitLimit { get; set; }
        public int QueueLimit { get; set; }
        public int SegmentsPerWindow { get; set; }
    }

    public class LoginLimiterOptions {
        public int WindowMinutes { get; set; }
        public int PermitLimit { get; set; }
        public int QueueLimit { get; set; }
        public int SegmentsPerWindow { get; set; }
    }
}
