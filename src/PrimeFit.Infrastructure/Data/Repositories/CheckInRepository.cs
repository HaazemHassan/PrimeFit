using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.Repositories;

namespace PrimeFit.Infrastructure.Data.Repositories
{
    internal class CheckInRepository : GenericRepository<CheckIn>, ICheckInRepository
    {
        private readonly DbSet<CheckIn> _checkIns;

        public CheckInRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
            _checkIns = context.Set<CheckIn>();
        }

        public Task<List<int>> GetAttendedDaysAsync(int subscriptionId, DateTimeOffset start, DateTimeOffset end, CancellationToken ct)
        {
            return _checkIns
                .AsNoTracking()
                .Where(c => c.SubscriptionId == subscriptionId && c.CreatedAt >= start && c.CreatedAt < end)
                .Select(c => c.CreatedAt.Day)
                .Distinct()
                .OrderBy(day => day)
                .ToListAsync(ct);
        }
    }
}