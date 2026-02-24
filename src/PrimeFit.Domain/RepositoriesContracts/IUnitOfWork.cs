using PrimeFit.Domain.RepositoriesContracts;

namespace PrimeFit.Domain.Repositories;

public interface IUnitOfWork
{

    IUserRepository Users { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    IBranchRepository Branches { get; }
    IBranchImageRepository BranchImages { get; }
    IBranchReviewRepository BranchReviews { get; }
    IBranchWorkingHourRepository BranchWorkingHours { get; }
    IGovernorateRepository Governorates { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<IDatabaseTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    bool HasCurrentTransaction();

    IDatabaseExecutionStrategy CreateExecutionStrategy();

}
