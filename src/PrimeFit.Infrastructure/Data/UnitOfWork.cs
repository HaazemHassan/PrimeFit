using Microsoft.EntityFrameworkCore.Storage;
using PrimeFit.Domain.Repositories;
using PrimeFit.Domain.RepositoriesContracts;
using PrimeFit.Infrastructure.Data.Transactions;

namespace PrimeFit.Infrastructure.Data;

internal sealed class UnitOfWork(
    AppDbContext context,
    IUserRepository users,
    IRefreshTokenRepository refreshTokens,
    IBranchRepository branches,
    IBranchImageRepository branchImages,
    IBranchReviewRepository branchReviews,
    IBranchWorkingHourRepository branchWorkingHours,
    IGovernorateRepository governorates,
    IPackageRepository packages,
    ISubscriptionRepository subscriptions,
    ISubscriptionFreezeRepository subscriptionFreezes,
    ICheckInRepository checkIns,
    IEmployeeRepository employees,
    IVerificationCodeRepository verificationCodes)
    : IUnitOfWork
{
    private readonly AppDbContext _context = context;

    public IUserRepository Users { get; } = users;
    public IRefreshTokenRepository RefreshTokens { get; } = refreshTokens;
    public IBranchRepository Branches { get; } = branches;
    public IBranchImageRepository BranchImages { get; } = branchImages;
    public IBranchReviewRepository BranchReviews { get; } = branchReviews;
    public IBranchWorkingHourRepository BranchWorkingHours { get; } = branchWorkingHours;
    public IGovernorateRepository Governorates { get; } = governorates;
    public IPackageRepository Packages { get; } = packages;
    public ISubscriptionRepository Subscriptions { get; } = subscriptions;
    public ISubscriptionFreezeRepository SubscriptionFreezes { get; } = subscriptionFreezes;
    public ICheckInRepository CheckIns { get; } = checkIns;
    public IEmployeeRepository Employees { get; } = employees;
    public IVerificationCodeRepository VerificationCodes { get; } = verificationCodes;

    private IDbContextTransaction? _transaction;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IDatabaseTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        return new DatabaseTransaction(_transaction);
    }

    public bool HasCurrentTransaction()
    {
        return _context.Database.CurrentTransaction != null;
    }

    public IDatabaseExecutionStrategy CreateExecutionStrategy()
    {
        var strategy = _context.Database.CreateExecutionStrategy();

        return new DatabaseExecutionStrategy(strategy);
    }
}