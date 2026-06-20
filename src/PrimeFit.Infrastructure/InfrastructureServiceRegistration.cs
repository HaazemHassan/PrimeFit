using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrimeFit.Application.Common.Idempotency;
using PrimeFit.Application.Common.Options;
using PrimeFit.Application.Contracts.Infrastructure;
using PrimeFit.Application.Security.Contracts;
using PrimeFit.Application.ServicesContracts.Infrastructure;
using PrimeFit.Application.ServicesContracts.Infrastructure.Payments;
using PrimeFit.Application.ServicesContracts.Infrastructure.Cashing;
using PrimeFit.Domain.Repositories;
using PrimeFit.Domain.RepositoriesContracts;
using PrimeFit.Infrastructure.BackgroundJobs;
using PrimeFit.Infrastructure.BackgroundJobs.Jobs;
using PrimeFit.Infrastructure.Cashing;
using PrimeFit.Infrastructure.Common.Options;
using PrimeFit.Infrastructure.Data;
using PrimeFit.Infrastructure.Data.Identity.Entities;
using PrimeFit.Infrastructure.Data.Interceptors;
using PrimeFit.Infrastructure.Data.Repositories;
using PrimeFit.Infrastructure.Data.Seeding;
using PrimeFit.Infrastructure.Emails;
using PrimeFit.Infrastructure.Health;
using PrimeFit.Infrastructure.Idempotency;
using PrimeFit.Infrastructure.Notifications;
using PrimeFit.Infrastructure.Payments;
using PrimeFit.Infrastructure.Security;
using PrimeFit.Infrastructure.Services;
using PrimeFit.Infrastructure.Storage;

namespace PrimeFit.Infrastructure;

public static class InfrastructureServiceRegistration
{

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        AddDbContextConfiguations(services, configuration);
        AddIdentityConfigurations(services, configuration);
        AddCashing(services, configuration);
        AddRepositories(services);
        AddServices(services, configuration);
        AddHangfireConfiguration(services, configuration);
        AddBackgroundJobs(services);
        AddHealthChecks(services, configuration);
        AddFirebase(services, configuration);

        return services;
    }

    private static void AddHealthChecks(IServiceCollection services, IConfiguration configuration)
    {

        string dbConnectionString = configuration.GetConnectionString("DefaultConnection")!;
        string redisConnectionString = configuration.GetConnectionString("Redis")!;

        services.AddHealthChecks()
                .AddSqlServer(dbConnectionString)
                //.AddRedis(
                //    redisConnectionString,
                //    name: "redis",
                //    tags: ["caching", "external"]
                //)
                .AddCheck<CloudinaryHealthCheck>(
                    name: "cloudinary",
                    tags: ["storage", "external"]
                 );
    }

    private static void AddDbContextConfiguations(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(
                configuration["ConnectionStrings:DefaultConnection"],
                x =>
                {
                    x.UseNetTopologySuite();
                    x.MigrationsHistoryTable("__EFMigrationsHistory", "ef");
                });

            options.AddInterceptors(serviceProvider.GetRequiredService<AuditingInterceptor>());
        });

        services.AddScoped<AuditingInterceptor>();

    }


    private static IServiceCollection AddIdentityConfigurations(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppPasswordOptions>(configuration.GetSection(AppPasswordOptions.SectionName));

        var passwordOptions = configuration.GetSection(AppPasswordOptions.SectionName).Get<AppPasswordOptions>();

        services.AddIdentity<ApplicationUser, ApplicationRole>(option =>
        {
            // Password settings 
            option.Password.RequireDigit = passwordOptions.RequireDigit;
            option.Password.RequireLowercase = passwordOptions.RequireLowercase;
            option.Password.RequireNonAlphanumeric = passwordOptions.RequireNonAlphanumeric;
            option.Password.RequireUppercase = passwordOptions.RequireUppercase;
            option.Password.RequiredLength = passwordOptions.MinLength;
            option.Password.RequiredUniqueChars = passwordOptions.RequiredUniqueChars;

            // Lockout settings.
            option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            option.Lockout.MaxFailedAccessAttempts = 5;
            option.Lockout.AllowedForNewUsers = true;

            // User settings.
            option.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            option.User.RequireUniqueEmail = true;
            option.SignIn.RequireConfirmedEmail = false;


        }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

        return services;
    }

    private static IServiceCollection AddHangfireConfiguration(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<HangfireOptions>(configuration.GetSection(HangfireOptions.SectionName));

        var hangfireOptions = configuration.GetSection(HangfireOptions.SectionName).Get<HangfireOptions>();

        services.AddHangfire(config => config
         .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
         .UseSimpleAssemblyNameTypeSerializer()
         .UseRecommendedSerializerSettings()
         .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"), new Hangfire.SqlServer.SqlServerStorageOptions
         {
             SchemaName = "jobs"
         }));

        services.AddHangfireServer();

        return services;
    }

    private static IServiceCollection AddBackgroundJobs(IServiceCollection services)
    {
        services.AddScoped<RefreshTokensCleanupJob>();
        services.AddScoped<IImageBackgroundService, HangfireImageBackgroundService>();
        services.AddScoped<OrphanedImagesCleanupJob>();

        return services;
    }


    private static IServiceCollection AddCashing(IServiceCollection services, IConfiguration configuration)
    {
        //services.AddStackExchangeRedisCache(options =>
        //{
        //    var redisOptions = ConfigurationOptions.Parse(configuration.GetConnectionString("Redis")!);

        //    redisOptions.AbortOnConnectFail = false;
        //    redisOptions.ConnectRetry = 3;
        //    redisOptions.ConnectTimeout = 5000;
        //    redisOptions.SyncTimeout = 5000;
        //    redisOptions.KeepAlive = 60;
        //    redisOptions.ReconnectRetryPolicy = new ExponentialRetry(5000);
        //    options.ConfigurationOptions = redisOptions;
        //    options.InstanceName = "PrimeFit_";

        //});

        services.AddHybridCache(options =>
        {
            options.MaximumPayloadBytes = 1 * 1024 * 1024;

            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(5),
                LocalCacheExpiration = TimeSpan.FromMinutes(1)
            };
        });

        services.AddScoped<ICacheService, HybridCacheService>();

        return services;
    }


    private static IServiceCollection AddRepositories(IServiceCollection services)
    {

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IBranchRepository, BranchRepository>();
        services.AddScoped<IBranchReviewRepository, BranchReviewRepository>();
        services.AddScoped<IBranchWorkingHourRepository, BranchWorkingHourRepository>();
        services.AddScoped<IGovernorateRepository, GovernorateRepository>();
        services.AddScoped<IBranchImageRepository, BranchImageRepostiory>();
        services.AddScoped<IPackageRepository, PackageRepository>();
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        services.AddScoped<ISubscriptionFreezeRepository, SubscriptionFreezeRepository>();
        services.AddScoped<ICheckInRepository, CheckInRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IVerificationCodeRepository, VerificationCodeRepository>();
        services.AddScoped<IUserDeviceTokenRepository, UserDeviceTokenRepository>();
        services.AddScoped<IPaymentTransactionRepository, PaymentTransactionRepository>();





        return services;
    }

    private static IServiceCollection AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISeederService, SeederService>();
        services.AddScoped<IApplicationUserService, ApplicationUserService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IEmailVerificationService, EmailVerificationService>();
        services.AddScoped<IPolicyEnforcer, PolicyEnforcer>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<IBranchAuthorizationService, BranchAuthorizationService>();
        services.AddScoped<IIdempotencyService, IdempotencyService>();


        services.AddSingleton<IPhoneNumberService, PhoneNumberService>();
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        services.AddSingleton<ITotpService, TotpService>();
        services.AddSingleton<IOtpService, OtpService>();


        services.Configure<GoogleAuthOptions>(configuration.GetSection(GoogleAuthOptions.SectionName));
        services.AddScoped<IGoogleAuthService, GoogleAuthService>();

        services.Configure<CloudinaryOptions>(configuration.GetSection(CloudinaryOptions.SectionName));
        services.AddScoped<IImageService, CloudinaryImageService>();


        services.Configure<EmailVerificationCodeOptions>(configuration.GetSection(EmailVerificationCodeOptions.SectionName));
        services.Configure<MailOptions>(configuration.GetSection(MailOptions.SectionName));

        services.AddSingleton<IEmailBodyBuilderService, EmailBodyBuilderService>();
        services.AddScoped<IEmailService, EmailService>();


        services.Configure<StripeOptions>(configuration.GetSection(StripeOptions.SectionName));
        services.AddScoped<IPaymentService, StripePaymentService>();

        return services;
    }

    private static void AddFirebase(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FirebaseOptions>(configuration.GetSection(FirebaseOptions.SectionName));
        services.AddSingleton<IPushNotificationService, FirebasePushNotificationService>();
    }

}
