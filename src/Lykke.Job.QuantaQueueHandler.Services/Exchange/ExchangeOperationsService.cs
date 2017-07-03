using System;
using System.Threading.Tasks;
using Common;
using Lykke.Job.QuantaQueueHandler.Core.Domain.BitCoin;
using Lykke.Job.QuantaQueueHandler.Core.Domain.BitCoin.TransactionContextModels;
using Lykke.MatchingEngine.Connector.Abstractions.Services;

namespace Lykke.Job.QuantaQueueHandler.Services.Exchange
{
    public class ExchangeOperationsService
    {
        private readonly IMatchingEngineClient _matchingEngineConnector;
        private readonly IBitCoinTransactionsRepository _bitCoinTransactionsRepository;


        public ExchangeOperationsService(IMatchingEngineClient matchingEngineConnector,
            IBitCoinTransactionsRepository bitCoinTransactionsRepository)
        {
            _matchingEngineConnector = matchingEngineConnector;
            _bitCoinTransactionsRepository = bitCoinTransactionsRepository;
        }

        public async Task<ResultModel> IssueAsync(string clientId, string assetId, double amount)
        {
            var transactionId = Guid.NewGuid().ToString();

            //create transaction and place context
            var contextData = new IssueContextData
            {
                Amount = amount,
                AssetId = assetId,
                ClientId = clientId,
                SignsClientIds = new[] { clientId }
            };

            await _bitCoinTransactionsRepository.CreateAsync(transactionId, BitCoinCommands.Issue,
                "", contextData.ToJson(), "");

            //Send to Matchin Engine
            var result = await _matchingEngineConnector
                .CashInOutAsync(transactionId, clientId, assetId, Math.Abs(amount));

            return result.ToDomainModel();
        }       
    }
}