using System.Threading.Tasks;
using AzureStorage;
using Lykke.Job.QuantaQueueHandler.Core.Domain.BitCoin;

namespace Lykke.Job.QuantaQueueHandler.AzureRepositories.BitCoin
{
    public class WalletCredentialsRepository : IWalletCredentialsRepository
    {
        private readonly INoSQLTableStorage<WalletCredentialsEntity> _tableStorage;

        public WalletCredentialsRepository(INoSQLTableStorage<WalletCredentialsEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IWalletCredentials> GetByQuantaContractAsync(string contract)
        {
            var partitionKey = WalletCredentialsEntity.ByQuantaContract.GeneratePartitionKey();
            var rowKey = WalletCredentialsEntity.ByQuantaContract.GenerateRowKey(contract);

            return await _tableStorage.GetDataAsync(partitionKey, rowKey);
        }
    }
}