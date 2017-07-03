
using System;

namespace Lykke.Job.QuantaQueueHandler.Models
{
    public class IsAliveResponse
    {
        public string Version { get; set; }
        public string Env { get; set; }
        public DateTime LastMessageProcessingStartedMoment { get; set; }
        public TimeSpan LastMessageProcessingDuration { get; set; }
        public TimeSpan MessageProcessingIdleDuration { get; set; }
        public int MessageProcessingFailedInARow { get; set; }
        public TimeSpan MaxHealthyMessageProcessingDuration { get; set; }
        public TimeSpan MaxHealthyMessageProcessingIsIdleDuration { get; set; }
        public int MaxHealthyMessageProcessingFailedInARow { get; set; }
    }
}