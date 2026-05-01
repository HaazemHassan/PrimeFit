using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrimeFit.Domain.Entities;
using PrimeFit.Infrastructure.Data.Filters;
using PrimeFit.Infrastructure.Data.Identity.Entities;
using System.Reflection;

namespace PrimeFit.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, IdentityUserClaim<int>, IdentityUserRole<int>, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {

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
        public DbSet<VerificationCode> VerificationCodes => Set<VerificationCode>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            SoftDeleteQueryFilter.Apply(modelBuilder);
        }
    }
}