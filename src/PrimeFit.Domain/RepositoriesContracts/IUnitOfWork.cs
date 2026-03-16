using PrimeFit.Domain.Repositories;

namespace PrimeFit.Domain.RepositoriesContracts;

public interface IUnitOfWork
{

    IUserRepository Users { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    IBranchRepository Branches { get; }
    IBranchImageRepository BranchImages { get; }
    IBranchReviewRepository BranchReviews { get; }
    IBranchWorkingHourRepository BranchWorkingHours { get; }
    IGovernorateRepository Governorates { get; }
    IPackageRepository Packages { get; }
    ISubscriptionRepository Subscriptions { get; }
    ISubscriptionFreezeRepository SubscriptionFreezes { get; }
    ICheckInRepository CheckIns { get; }
    IEmployeeRepository Employees { get; }
    IVerificationCodeRepository VerificationCodes { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<IDatabaseTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    bool HasCurrentTransaction();

    IDatabaseExecutionStrategy CreateExecutionStrategy();

}
