using PrimeFit.Domain.Entities;

namespace PrimeFit.Domain.Repositories
{
    public interface IUserDeviceTokenRepository : IGenericRepository<UserDeviceToken>
    {
        Task<UserDeviceToken?> GetByTokenAsync(string token, CancellationToken ct);
    }
}
