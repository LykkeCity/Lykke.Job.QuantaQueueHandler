using System.Threading.Tasks;

namespace Lykke.Job.QuantaQueueHandler.Core.Domain.BitCoin
{
    public interface IBitCoinTransactionsRepository
    {
        Task CreateAsync(string transactionId, string commandType, string requestData, string contextData, string response, string blockchainHash = null);
    }
}