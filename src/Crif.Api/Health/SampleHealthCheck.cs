using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;

namespace Crif.Api
{
    public class SampleHealthCheck : HealthCheck
    {
        public SampleHealthCheck() : base("Sample Health Check")
        {
            
        }

        protected override Task<HealthCheckResult> CheckAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(HealthCheckResult.Healthy());
        }
    }
}