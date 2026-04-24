using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PrimeFit.Application.Common;
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
                cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
            });


            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


            AddAuthorizationPolicies(services);
            AddCashingPolicies(services);

            return services;
        }


        private static void AddAuthorizationPolicies(IServiceCollection services)
        {
            services.AddScoped<IAuthorizationPolicy, SelfOrAdminPolicy>();
            services.AddScoped<IAuthorizationPolicy, SelfOnlyPolicy>();
            services.AddScoped<IAuthorizationPolicy, BranchStaffOnlyPolicy>();

        }


        private static void AddCashingPolicies(IServiceCollection services)
        {
            var assembly = typeof(ICachePolicy<>).Assembly;

            var policyInterfaceType = typeof(ICachePolicy<>);

            var types = assembly.GetTypes()
                .Where(t => t is { IsClass: true, IsAbstract: false })
                .SelectMany(t => t.GetInterfaces(), (t, i) => new { Implementation = t, Interface = i })
                .Where(x =>
                    x.Interface.IsGenericType &&
                    x.Interface.GetGenericTypeDefinition() == policyInterfaceType);

            foreach (var type in types)
            {
                services.AddTransient(type.Interface, type.Implementation);
            }

        }

    }
}