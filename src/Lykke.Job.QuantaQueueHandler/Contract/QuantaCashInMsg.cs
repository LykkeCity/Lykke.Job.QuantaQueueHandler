namespace Lykke.Job.QuantaQueueHandler.Contract
{
    public class QuantaCashInMsg
    {
        public string Contract { get; set; }
        public double Amount { get; set; }
        public string TransactionHash { get; set; }
    }
}