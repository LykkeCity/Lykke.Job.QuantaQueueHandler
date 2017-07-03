using System;
using Common;
using Lykke.Job.QuantaQueueHandler.Core.Domain.PaymentSystems;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Job.QuantaQueueHandler.AzureRepositories.PaymentSystems
{
    public class PaymentTransactionEntity : TableEntity, IPaymentTransaction
    {

        public static class IndexCommon
        {
            public static string GeneratePartitionKey()
            {
                return "BCO";
            }

        }

        public static class IndexByClient
        {
            public static string GeneratePartitionKey(string clientId)
            {
                return clientId;
            }

            public static string GenerateRowKey(string orderId)
            {
                return orderId;
            }

        }

        public int Id { get; set; }
        public string TransactionId { get; set; }
        string IPaymentTransaction.Id => TransactionId ?? Id.ToString();

        public string ClientId { get; set; }
        public DateTime Created { get; set; }

        public string Status { get; set; }

        internal void SetPaymentStatus(PaymentStatus data)
        {
            Status = data.ToString();
        }

        internal PaymentStatus GetPaymentStatus()
        {
            return Utils.ParseEnum((string) Status, PaymentStatus.Created);
        }
        PaymentStatus IPaymentTransaction.Status => GetPaymentStatus();



        public string PaymentSystem { get; set; }
        public string Info { get; set; }
        CashInPaymentSystem IPaymentTransaction.PaymentSystem => GetPaymentSystem();

        internal void SetPaymentSystem(CashInPaymentSystem data)
        {
            PaymentSystem = data.ToString();
        }

        internal CashInPaymentSystem GetPaymentSystem()
        {
            return Utils.ParseEnum((string) PaymentSystem, CashInPaymentSystem.Unknown);
        }


        public double? Rate { get; set; }
        public string AggregatorTransactionId { get; set; }
        public double Amount { get; set; }
        public string AssetId { get; set; }
        public double? DepositedAmount { get; set; }
        public string DepositedAssetId { get; set; }

        public static PaymentTransactionEntity Create(IPaymentTransaction src)
        {
            var result = new PaymentTransactionEntity
            {
                Created = src.Created,
                TransactionId = src.Id,
                Info = src.Info,
                ClientId = src.ClientId,
                AssetId = src.AssetId,
                Amount = src.Amount,
                AggregatorTransactionId = src.AggregatorTransactionId,
                DepositedAssetId = src.DepositedAssetId
            };

            result.SetPaymentStatus(src.Status);

            result.SetPaymentSystem(src.PaymentSystem);

            return result;
        }

    }
}