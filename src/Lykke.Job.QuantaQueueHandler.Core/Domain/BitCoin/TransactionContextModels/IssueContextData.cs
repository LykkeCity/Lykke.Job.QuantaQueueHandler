namespace Lykke.Job.QuantaQueueHandler.Core.Domain.BitCoin.TransactionContextModels
{
    public class IssueContextData : BaseContextData
    {
        public string ClientId { get; set; }
        public string AssetId { get; set; }
        public double Amount { get; set; }

        public string CashOperationId { get; set; }
    }
}