
using System;

namespace Lykke.Job.QuantaQueueHandler.Core.Services
{
    public interface IHealthService
    {
        DateTime LastMessageProcessingStartedMoment { get; }
        TimeSpan LastMessageProcessingDuration { get; }
        TimeSpan MaxHealthyMessageProcessingDuration { get; }
        int MaxHealthyMessageProcessingFailedInARow { get; }
        TimeSpan MaxHealthyMessageProcessingIdleDuration { get; }
        int MessageProcessingFailedInARow { get; }
        TimeSpan MessageProcessingIdleDuration { get; }

        string GetHealthViolationMessage();
        void TraceMessageProcessingStarted();
        void TraceMessageProcessingCompleted();
        void TraceMessageProcessingFailed();
    }
}