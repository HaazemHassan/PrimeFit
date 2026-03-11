using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PrimeFit.Application.Common.Behaviors;
using PrimeFit.Application.Common.Behaviors.Transaction;
using PrimeFit.Application.Common.Behaviors.Trimming;
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
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            AddBehaviours(services);
            AddAuthorizationPolicies(services);
            AddDomainServices(services);

            return services;
        }



        private static void AddBehaviours(IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TrimmingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        }


        private static void AddAuthorizationPolicies(IServiceCollection services)
        {
            services.AddScoped<IAuthorizationPolicy, SelfOrAdminPolicy>();
            services.AddScoped<IAuthorizationPolicy, SelfOnlyPolicy>();
            services.AddScoped<IAuthorizationPolicy, BranchStaffOnlyPolicy>();

        }



        private static void AddDomainServices(IServiceCollection services)
        {
        }
    }
}