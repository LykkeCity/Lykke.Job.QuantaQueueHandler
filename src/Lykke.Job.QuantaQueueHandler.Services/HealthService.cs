using Lykke.Job.QuantaQueueHandler.Core.Services;

namespace Lykke.Job.QuantaQueueHandler.Services
{
    public class HealthService : IHealthService
    {
        public string GetHealthViolationMessage()
        {
            // TODO: Check gathered health statistics, and return appropriate health violation message, or NULL if job is ok
            return null;
        }
    }
}