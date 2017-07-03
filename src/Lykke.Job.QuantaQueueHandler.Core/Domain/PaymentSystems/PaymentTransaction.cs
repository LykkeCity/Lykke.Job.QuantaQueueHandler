using System;

namespace Lykke.Job.QuantaQueueHandler.Core.Domain.PaymentSystems
{
    public class PaymentTransaction : IPaymentTransaction
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public double Amount { get; set; }
        public string AssetId { get; set; }
        public double? DepositedAmount { get; set; }
        public string DepositedAssetId { get; set; }
        public double? Rate { get; set; }
        public string AggregatorTransactionId { get; set; }
        public DateTime Created { get; set; }
        public PaymentStatus Status { get; set; }
        public CashInPaymentSystem PaymentSystem { get; set; }
        public string Info { get; set; }

        public string OtherData { get; set; }


        public static PaymentTransaction Create(string id,
            CashInPaymentSystem paymentSystem,
            string clientId,
            double amount,
            string assetId,
            string assetToDeposit = null,
            string info = "",
            PaymentStatus status = PaymentStatus.Created)
        {

            return new PaymentTransaction
            {
                Id = id,
                PaymentSystem = paymentSystem,
                ClientId = clientId,
                Amount = amount,
                AssetId = assetId,
                Created = DateTime.UtcNow,
                Status = status,
                Info = info,
                DepositedAssetId = assetToDeposit ?? assetId
            };
        }
    }
}