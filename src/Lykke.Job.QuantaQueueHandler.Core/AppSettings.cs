using System;
using System.Net;

namespace Lykke.Job.QuantaQueueHandler.Core
{
    public class AppSettings
    {
        public QuantaQueueHandlerSettings QuantaQueueHandlerJob { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }

        public class QuantaQueueHandlerSettings
        {
            public DbSettings Db { get; set; }
            public MatchingOrdersSettings MatchingEngine { get; set; }
            public HealthSettings Health { get; set; }
            public string TriggerQueueConnectionString { get; set; }
        }

        public class DbSettings
        {
            public string LogsConnString { get; set; }
            public string BitCoinQueueConnectionString { get; set; }
            public string ClientPersonalInfoConnString { get; set; }
        }

        public class MatchingOrdersSettings
        {
            public IpEndpointSettings IpEndpoint { get; set; }
        }

        public class HealthSettings
        {
            public TimeSpan MaxMessageProcessingDuration { get; set; }
            public int MaxMessageProcessingFailedInARow { get; set; }
        }

        public class IpEndpointSettings
        {
            public string Host { get; set; }
            public int Port { get; set; }

            public IPEndPoint GetClientIpEndPoint(bool useInternal = false)
            {
                return new IPEndPoint(IPAddress.Parse(Host), Port);
            }
        }

        public class SlackNotificationsSettings
        {
            public AzureQueueSettings AzureQueue { get; set; }

            public int ThrottlingLimitSeconds { get; set; }
        }

        public class AzureQueueSettings
        {
            public string ConnectionString { get; set; }

            public string QueueName { get; set; }
        }
    }
}