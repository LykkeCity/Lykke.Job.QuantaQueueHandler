using System.Threading.Tasks;

namespace Lykke.Job.QuantaQueueHandler.Core.Domain.BitCoin
{
    public interface IWalletCredentialsRepository
    {
        Task<IWalletCredentials> GetByQuantaContractAsync(string contract);
    }
}