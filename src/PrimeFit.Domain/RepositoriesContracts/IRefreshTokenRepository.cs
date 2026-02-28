using PrimeFit.Domain.Entities;

namespace PrimeFit.Domain.Repositories
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        Task DeleteExpiredTokensAsync(DateTimeOffset cutoffDate);
    }
}
