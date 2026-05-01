using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using PrimeFit.Infrastructure.Storage;

namespace PrimeFit.Infrastructure.Health
{

    public class CloudinaryHealthCheck : IHealthCheck
    {
        private readonly CloudinaryOptions _options;

        public CloudinaryHealthCheck(IOptions<CloudinaryOptions> options)
        {
            _options = options.Value;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var account = new Account(
                    _options.CloudName,
                    _options.ApiKey,
                    _options.ApiSecret
                );

                var cloudinary = new Cloudinary(account);

                cloudinary.Api.Secure = true;

                var pingResult = await cloudinary.ListResourcesAsync(new ListResourcesParams { MaxResults = 1 }, cancellationToken);

                if (pingResult.Error is not null)
                {
                    return HealthCheckResult.Degraded(description: $"Cloudinary returned error: {pingResult.Error.Message}");
                }

                return HealthCheckResult.Healthy("Cloudinary is reachable and credentials are valid.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Degraded(description: "Cloudinary health check failed.", exception: ex);
            }
        }
    }

}
