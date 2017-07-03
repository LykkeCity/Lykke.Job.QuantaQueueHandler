using System;
using Lykke.Job.QuantaQueueHandler.Core.Domain.PaymentSystems;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Job.QuantaQueueHandler.AzureRepositories.PaymentSystems
{
    public class PaymentSystemRawLogEventEntity : TableEntity, IPaymentSystemRawLogEvent
    {
        public static string GeneratePartitionKey(string paymentSystem)
        {
            return paymentSystem;
        }

        public DateTime DateTime { get; set; }
        public string PaymentSystem => PartitionKey;
        public string EventType { get; set; }
        public string Data { get; set; }

        public static PaymentSystemRawLogEventEntity Create(IPaymentSystemRawLogEvent src)
        {
            return new PaymentSystemRawLogEventEntity
            {
                PartitionKey = GeneratePartitionKey(src.PaymentSystem),
                DateTime = src.DateTime,
                Data = src.Data,
                EventType = src.EventType
            };
        }

    }
}