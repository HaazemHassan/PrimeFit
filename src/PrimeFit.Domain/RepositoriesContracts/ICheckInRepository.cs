using PrimeFit.Domain.Entities;

namespace PrimeFit.Domain.Repositories
{
    public interface ICheckInRepository : IGenericRepository<CheckIn>
    {
        Task<List<int>> GetAttendedDaysAsync(int subscriptionId, DateTimeOffset start, DateTimeOffset end, CancellationToken ct);
    }
}