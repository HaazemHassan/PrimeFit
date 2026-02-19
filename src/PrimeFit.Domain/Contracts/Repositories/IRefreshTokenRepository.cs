using PrimeFit.Domain.Entities;

namespace PrimeFit.Domain.Contracts.Repositories
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        Task DeleteExpiredTokensAsync(DateTime cutoffDate);
    }
}
