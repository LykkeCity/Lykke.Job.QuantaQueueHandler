using System;

namespace Lykke.Job.QuantaQueueHandler.Core.Domain.PaymentSystems
{
    public class PaymentSystemRawLogEvent : IPaymentSystemRawLogEvent
    {
        public DateTime DateTime { get; set; }
        public string PaymentSystem { get; set; }
        public string EventType { get; set; }
        public string Data { get; set; }

        public static PaymentSystemRawLogEvent Create(CashInPaymentSystem paymentSystem, string eventType, string data)
        {
            return new PaymentSystemRawLogEvent
            {
                DateTime = DateTime.UtcNow,
                PaymentSystem = paymentSystem.ToString(),
                Data = data,
                EventType = eventType
            };
        }

    }
}