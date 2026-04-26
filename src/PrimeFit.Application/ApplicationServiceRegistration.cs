using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PrimeFit.Application.Common;
using PrimeFit.Application.Common.Caching;
using PrimeFit.Application.Common.Cashing;
using PrimeFit.Application.Common.Logging;
using PrimeFit.Application.Common.Transaction;
using PrimeFit.Application.Common.Trimming;
using PrimeFit.Application.Common.Validation;
using PrimeFit.Application.Security;
using PrimeFit.Application.Security.Policies;
using System.Reflection;

namespace PrimeFit.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {

            services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);

                cfg.AddOpenBehavior(typeof(TrimmingBehavior<,>));
                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
                cfg.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(CachingBehavior<,>));
                cfg.AddOpenBehavior(typeof(CacheInvalidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
            });


            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


            AddAuthorizationPolicies(services);
            AddCachePolicies(services, typeof(ICachePolicy<>));
            AddCachePolicies(services, typeof(ICacheInvalidationPolicy<>));

            return services;
        }


        private static void AddAuthorizationPolicies(IServiceCollection services)
        {
            services.AddScoped<IAuthorizationPolicy, SelfOrAdminPolicy>();
            services.AddScoped<IAuthorizationPolicy, SelfOnlyPolicy>();
            services.AddScoped<IAuthorizationPolicy, BranchStaffOnlyPolicy>();

        }


        private static void AddCachePolicies(IServiceCollection services, Type openGenericPolicyInterface)
        {
            var assembly = typeof(ApplicationAssemblyMarker).Assembly;

            var implementations = assembly.GetTypes()
                .Where(t => t is { IsClass: true, IsAbstract: false })
                .SelectMany(t => t.GetInterfaces(),
                    (implementation, iface) => new
                    {
                        Implementation = implementation,
                        Interface = iface
                    })
                .Where(x =>
                    x.Interface.IsGenericType &&
                    x.Interface.GetGenericTypeDefinition() == openGenericPolicyInterface);

            foreach (var type in implementations)
            {
                services.AddTransient(type.Interface, type.Implementation);
            }
        }

    }
}