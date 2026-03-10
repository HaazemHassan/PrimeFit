using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrimeFit.Application.Contracts.Api;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Domain.Entities;
using PrimeFit.Domain.Entities.Contracts;
using PrimeFit.Infrastructure.Data.Identity.Entities;
using System.Linq.Expressions;
using System.Reflection;

namespace PrimeFit.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, IdentityUserClaim<int>, IdentityUserRole<int>, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;


        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<Branch> Branches => Set<Branch>();
        public DbSet<BranchWorkingHour> BranchWorkingHours => Set<BranchWorkingHour>();
        public DbSet<BranchReview> BranchReviews => Set<BranchReview>();
        public DbSet<BranchImage> BranchImages => Set<BranchImage>();
        public DbSet<Governorate> Governorates => Set<Governorate>();
        public DbSet<Package> Packages => Set<Package>();
        public DbSet<Subscription> Subscriptions => Set<Subscription>();
        public DbSet<SubscriptionFreeze> SubscriptionFreezes => Set<SubscriptionFreeze>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<EmployeeRole> EmployeeRoles => Set<EmployeeRole>();
        public DbSet<EmployeeRolePermission> EmployeeRolePermissions => Set<EmployeeRolePermission>();
        public DbSet<CheckIn> CheckIns => Set<CheckIn>();



        public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUserService, IDateTimeProvider dateTimeProvider) : base(options)
        {
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


            #region query filters
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeletableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(GenerateIsDeletedFilter(entityType.ClrType));
                }
            }
            #endregion
        }


        // Override SaveChangesAsync to automatically set audit properties  
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.UserId;
            var utcNow = _dateTimeProvider.UtcNow;

            foreach (var entry in ChangeTracker.Entries())
            {

                if (entry.State == EntityState.Added)
                {
                    if (entry.Entity is IHasCreationTime cTime) cTime.CreatedAt = utcNow;
                    if (entry.Entity is IHasCreator cUser) cUser.CreatedBy = userId;
                }

                else if (entry.State == EntityState.Modified)
                {
                    if (entry.Entity is IHasModificationTime mTime) mTime.UpdatedAt = utcNow;
                    if (entry.Entity is IHasModifier mUser) mUser.UpdatedBy = userId;

                    //prevent modification of creation properties
                    if (entry.Entity is IHasCreationTime) entry.Property(nameof(IHasCreationTime.CreatedAt)).IsModified = false;
                    if (entry.Entity is IHasCreator) entry.Property(nameof(IHasCreator.CreatedBy)).IsModified = false;

                }

                else if (entry.Entity is ISoftDeletableEntity softDelete && entry.State == EntityState.Deleted)
                {
                    if (softDelete.IsDeleted)
                    {
                        entry.State = EntityState.Unchanged;
                    }
                    else
                    {
                        entry.State = EntityState.Modified;
                        softDelete.IsDeleted = true;
                        softDelete.DeletedAt = utcNow;
                        softDelete.DeletedBy = userId;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        #region Helper Functions

        // Generates a lambda expression for the global query filter to exclude soft-deleted entities
        private static LambdaExpression GenerateIsDeletedFilter(Type type)
        {
            var parameter = Expression.Parameter(type, "it");
            var property = Expression.Property(parameter, nameof(IFullyAuditableEntity.IsDeleted));
            var falseConstant = Expression.Constant(false);
            var body = Expression.Equal(property, falseConstant);

            return Expression.Lambda(body, parameter);
        }

        #endregion
    }
}