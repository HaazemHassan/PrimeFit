using Hangfire;
using HangfireBasicAuthenticationFilter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PrimeFit.Infrastructure.BackgroundJobs;
using PrimeFit.Infrastructure.Data;
using PrimeFit.Infrastructure.Data.Seeding;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrimeFit.API.Extentions
{
    public static class WebApplicationExtensions
    {

        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            #region Initialize Database
            using (var scope = app.Services.CreateScope())
            {


                // needed to dockerize the application to make the DB created and updated automatically
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                try
                {
                    await context.Database.MigrateAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Database migration error: {ex.Message}");
                    throw;
                }



                var logger = app.Services.GetRequiredService<ILogger<Program>>();

                try
                {
                    var seederService = scope.ServiceProvider.GetRequiredService<ISeederService>();

                    string rolesJson = await File.ReadAllTextAsync("DataSeedingJson/Roles.json");
                    string usersJson = await File.ReadAllTextAsync("DataSeedingJson/Users.json");
                    string employeeRolesJson = await File.ReadAllTextAsync("DataSeedingJson/EmployeeRoles.json");

                    List<RoleSeedDto>? rolesSeedData = JsonSerializer.Deserialize<List<RoleSeedDto>>(rolesJson);

                    var options = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
                    List<UserSeedDto>? usersSeedData = JsonSerializer.Deserialize<List<UserSeedDto>>(usersJson, options);
                    List<EmployeeRoleSeedDto>? employeeRolesSeedData = JsonSerializer.Deserialize<List<EmployeeRoleSeedDto>>(employeeRolesJson);

                    await seederService.SeedRolesAsync(rolesSeedData!);
                    await seederService.SeedUsersAsync(usersSeedData!);
                    await seederService.SeedEmployeeRolesAsync(employeeRolesSeedData!);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while seeding the database.");
                    throw;

                }
                #endregion
            }
        }

        public static void UseCustomHangfireDashboard(this WebApplication app)
        {
            var hangfireSettings = app.Services.GetRequiredService<IOptions<HangfireOptions>>().Value;
            app.UseHangfireDashboard(hangfireSettings.DashboardPath, new DashboardOptions
            {
                Authorization =
                [
                    new HangfireCustomBasicAuthenticationFilter
                    {
                        User = hangfireSettings.Username,
                        Pass = hangfireSettings.Password
                    }
                ]
            });
        }

        public static void RegisterRecurringJobs(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var jobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
                jobManager.RegisterRecurringJobs();
            }
        }
    }
}
