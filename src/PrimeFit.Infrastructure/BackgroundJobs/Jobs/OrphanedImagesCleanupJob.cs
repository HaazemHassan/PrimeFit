using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.Common.Enums;
using PrimeFit.Infrastructure.Data;

namespace PrimeFit.Infrastructure.BackgroundJobs.Jobs
{
    public class OrphanedImagesCleanupJob
    {
        public const string JobId = "orphaned-images-cleanup";

        public static readonly string Schedule = Cron.Daily(hour: 3);

        private const int DbBatchSize = 100;

        private static readonly TimeSpan PendingGracePeriod = TimeSpan.FromDays(2);

        private readonly AppDbContext _dbContext;
        private readonly IImageService _imageService;
        private readonly ILogger<OrphanedImagesCleanupJob> _logger;
        private readonly TimeProvider _timeProvider;

        public OrphanedImagesCleanupJob(
            AppDbContext dbContext,
            IImageService imageService,
            ILogger<OrphanedImagesCleanupJob> logger,
            TimeProvider timeProvider)
        {
            _dbContext = dbContext;
            _imageService = imageService;
            _logger = logger;
            _timeProvider = timeProvider;
        }

        public async Task ExecuteAsync(CancellationToken ct = default)
        {
            var now = _timeProvider.GetUtcNow();
            var pendingCutoff = now - PendingGracePeriod;

            _logger.LogInformation(
                "[OrphanedImagesCleanup] Started. PendingCutoff={Cutoff}",
                pendingCutoff);

            int totalDeleted = 0;
            int totalFailed = 0;
            int iteration = 0;

            long lastId = 0;

            while (true)
            {
                ct.ThrowIfCancellationRequested();

                iteration++;

                // ─────────────────────────────────────────────
                // 1. Fetch batch using cursor pagination
                // ─────────────────────────────────────────────
                var batch = await _dbContext.BranchImages
                    .IgnoreQueryFilters()
                    .AsNoTracking()
                    .Where(i =>
                        i.Id > lastId &&
                        (
                            i.Status == BranchImageStatus.Replaced ||
                            (
                                i.Status == BranchImageStatus.Pending &&
                                i.CreatedAt < pendingCutoff
                            )
                        ))
                    .OrderBy(i => i.Id)
                    .Take(DbBatchSize)
                    .ToListAsync(ct);

                if (batch.Count == 0)
                {
                    _logger.LogInformation("[OrphanedImagesCleanup] Image are less than batch size. Stopping.");
                    break;
                }

                lastId = batch.Last().Id;

                _logger.LogInformation(
                    "[OrphanedImagesCleanup] Iteration {Iter}: Processing {Count} images.",
                    iteration,
                    batch.Count);

                // ─────────────────────────────────────────────
                // 2. Delete from Cloudinary
                // ─────────────────────────────────────────────
                var publicIds = batch
                    .Select(i => i.PublicId)
                    .Where(id => !string.IsNullOrWhiteSpace(id))
                    .ToList();

                if (publicIds.Count == 0)
                {
                    _logger.LogWarning(
                        "[OrphanedImagesCleanup] Batch had no valid publicIds.");
                    continue;
                }

                var deleteResult = await _imageService.DeleteRangeAsync(publicIds, ct);

                if (deleteResult.IsError)
                {
                    _logger.LogError(
                        "[OrphanedImagesCleanup] Cloudinary critical error: {Reason}. Aborting.",
                        deleteResult.FirstError.Description);

                    break;
                }

                var bulkResult = deleteResult.Value;

                // ─────────────────────────────────────────────
                // 3. Remove only successfully deleted rows from DB
                // ─────────────────────────────────────────────
                var failedSet = bulkResult.FailedPublicIds.ToHashSet();

                var toRemoveFromDb = batch.Where(i => !failedSet.Contains(i.PublicId)).ToList();

                if (toRemoveFromDb.Count == 0)
                {
                    _logger.LogWarning(
                        "[OrphanedImagesCleanup] No progress detected in iteration {Iter}. Stopping loop.",
                        iteration);

                    break;
                }


                var IdsToDelete = toRemoveFromDb.Select(i => i.Id).ToList();

                await _dbContext.BranchImages
                 .Where(i => IdsToDelete.Contains(i.Id))
                 .ExecuteDeleteAsync(ct);

                // ─────────────────────────────────────────────
                // 4. Memory safety
                // ─────────────────────────────────────────────
                _dbContext.ChangeTracker.Clear();

                totalDeleted += bulkResult.Deleted;
                totalFailed += bulkResult.FailedPublicIds.Count;

                if (bulkResult.FailedPublicIds.Count > 0)
                {
                    _logger.LogWarning(
                        "[OrphanedImagesCleanup] {FailCount} images failed deletion: {Ids}",
                        bulkResult.FailedPublicIds.Count,
                        string.Join(", ", bulkResult.FailedPublicIds));
                }

                // optimization exit condition
                if (batch.Count < DbBatchSize)
                {
                    _logger.LogInformation(
                        "[OrphanedImagesCleanup] Last partial batch reached.");
                    break;
                }
            }

            _logger.LogInformation(
                "[OrphanedImagesCleanup] Finished. TotalDeleted={Deleted}, TotalFailed={Failed}, Iterations={Iter}",
                totalDeleted,
                totalFailed,
                iteration);
        }
    }
}

