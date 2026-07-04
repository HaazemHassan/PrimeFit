using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrimeFit.Application.Common.DTOS;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Infrastructure.Data;

namespace PrimeFit.Infrastructure.Notifications.Jobs
{
    public class InactiveUsersNotificationJob
    {
        public const string JobId = "InactiveUsersNotificationJob";
        public static readonly string Schedule = Cron.Daily(12, 0);

        private const int BatchSize = 500;

        private readonly AppDbContext _dbContext;
        private readonly IPushNotificationService _pushNotificationService;
        private readonly ILogger<InactiveUsersNotificationJob> _logger;
        private readonly TimeProvider _timeProvider;

        public InactiveUsersNotificationJob(
            AppDbContext dbContext,
            IPushNotificationService pushNotificationService,
            ILogger<InactiveUsersNotificationJob> logger,
            TimeProvider timeProvider)
        {
            _dbContext = dbContext;
            _pushNotificationService = pushNotificationService;
            _logger = logger;
            _timeProvider = timeProvider;
        }

        public async Task ExecuteAsync(CancellationToken ct = default)
        {
            var now = _timeProvider.GetUtcNow();

            // Target users who were last active exactly 5 days ago.
            // That means their LastActivity was >= 5 days ago (start of day) and < 4 days ago (start of day)
            var targetStart = now.Date.AddDays(-5);
            var targetEnd = targetStart.AddDays(1);

            _logger.LogInformation("Starting InactiveUsersNotificationJob. Targeting users last active between {TargetStart} and {TargetEnd}", targetStart, targetEnd);

            var inactiveUserIdsQuery = _dbContext.Subscriptions
                .Where(s => s.Status == SubscriptionStatus.Active)
                .Select(s => new
                {
                    UserId = s.UserId,
                    LastActivityDate = _dbContext.CheckIns
                        .Where(c => c.CustomerId == s.UserId)
                        .Max(c => (DateTimeOffset?)c.CreatedAt) ?? s.ActivationDate
                })
                .Where(x => x.LastActivityDate != null && x.LastActivityDate >= targetStart && x.LastActivityDate < targetEnd)
                .Select(x => x.UserId)
                .Distinct();

            int skip = 0;
            int totalTargetUsers = 0;
            int totalTokensFound = 0;
            int totalSuccessfullySent = 0;

            while (true)
            {
                var userIdsBatch = await inactiveUserIdsQuery
                    .OrderBy(id => id)
                    .Skip(skip)
                    .Take(BatchSize)
                    .ToListAsync(ct);

                if (userIdsBatch.Count == 0)
                    break;

                totalTargetUsers += userIdsBatch.Count;

                var (tokensCount, successCount) = await ProcessBatchAsync(userIdsBatch, ct);
                totalTokensFound += tokensCount;
                totalSuccessfullySent += successCount;

                skip += BatchSize;
            }

            int totalFailed = totalTokensFound - totalSuccessfullySent;

            _logger.LogInformation(
                "Finished InactiveUsersNotificationJob. " +
                "Target Users: {TotalTargetUsers}, " +
                "Total Tokens Found: {TotalTokensFound}, " +
                "Successfully Sent: {TotalSuccessfullySent}, " +
                "Failed to Send: {TotalFailed}",
                totalTargetUsers, totalTokensFound, totalSuccessfullySent, totalFailed);
        }

        private async Task<(int TotalTokens, int SuccessCount)> ProcessBatchAsync(List<int> userIds, CancellationToken ct)
        {
            var userTokens = await _dbContext.UserDeviceTokens
                .Where(t => userIds.Contains(t.UserId))
                .Select(t => t.Token)
                .ToListAsync(ct);

            if (userTokens.Count == 0)
            {
                _logger.LogInformation("No tokens found for users");
                return (0, 0);
            }

            var request = new PushNotificationRequest
            {
                Title = "We Miss You",
                Body = "It's been a few days since your last workout. Let's get back on track today!"
            };

            int successCount = await _pushNotificationService.SendToDevicesAsync(request, userTokens, ct);

            _logger.LogInformation("Batch processed: {SuccessCount} successful out of {TotalTokens} tokens.", successCount, userTokens.Count);

            return (userTokens.Count, successCount);
        }
    }
}
