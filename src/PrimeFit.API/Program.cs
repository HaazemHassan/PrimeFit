using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using PrimeFit.API.Extentions;
using PrimeFit.API.Middlewares;
using Serilog;

namespace PrimeFit.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddDependencies(builder.Configuration, builder.Environment);
            builder.Host.UseSerilog((hostingContext, configuration) =>
            {
                configuration.ReadFrom.Configuration(hostingContext.Configuration);
            });


            var app = builder.Build();

            app.UseStaticFiles();

            if (app.Environment.IsDevelopment())
            {
                await app.InitializeDatabaseAsync();

                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.InjectStylesheet("/swagger-ui/dark.css");
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "PrimeFit API v1");


                });
                app.UseDeveloperExceptionPage();
            }

            app.MapHealthChecks(pattern: "api/health", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseExceptionHandler();
            app.UseSerilogRequestLogging();
            app.UseForwardedHeaders();
            app.UseSecurityHeaders();



            app.UseCustomHangfireDashboard();
            app.RegisterRecurringJobs();
            app.UseCors();

            app.UseGuestSession();
            app.UseAuthentication();
            app.UseAuthorization();

            if (app.Environment.IsProduction())
            {
                app.UseRateLimiter();
            }

            app.MapControllers();

            app.Run();
        }
    }
}
