using Lykke.Job.QuantaQueueHandler.Core.Domain.BitCoin.TransactionContextModels;

namespace Lykke.Job.QuantaQueueHandler.Core.Domain.BitCoin
{
    public static class BintCoinTransactionsRepositoryExt
    {
        public static T GetContextData<T>(this IBitcoinTransaction src)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(src.ContextData);
        }

        public static BaseContextData GetBaseContextData(this IBitcoinTransaction src)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<BaseContextData>(src.ContextData);
        }
    }
}