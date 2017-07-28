using System;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Service.ExchangeOperations.Contracts;
using Lykke.Job.QuantaQueueHandler.Contract;
using Lykke.Job.QuantaQueueHandler.Core;
using Lykke.Job.QuantaQueueHandler.Core.Domain.BitCoin;
using Lykke.Job.QuantaQueueHandler.Core.Domain.PaymentSystems;
using Lykke.Job.QuantaQueueHandler.Core.Services;
using Lykke.JobTriggers.Triggers.Attributes;

namespace Lykke.Job.QuantaQueueHandler.TriggerHandlers
{
    public class QuantaQueueHandler
    {
        private readonly IWalletCredentialsRepository _walletCredentialsRepository;
        private readonly ILog _log;
        private readonly IExchangeOperationsService _exchangeOperationsService;
        private readonly IPaymentSystemsRawLog _paymentSystemsRawLog;
        private readonly IPaymentTransactionsRepository _paymentTransactionsRepository;
        private readonly IHealthService _healthService;

        public QuantaQueueHandler(
            IWalletCredentialsRepository walletCredentialsRepository,
            ILog log,
            IExchangeOperationsService exchangeOperationsService,
            IPaymentSystemsRawLog paymentSystemsRawLog,
            IPaymentTransactionsRepository paymentTransactionsRepository,
            IHealthService healthService)
        {
            _walletCredentialsRepository = walletCredentialsRepository;
            _log = log;
            _exchangeOperationsService = exchangeOperationsService;
            _paymentSystemsRawLog = paymentSystemsRawLog;
            _paymentTransactionsRepository = paymentTransactionsRepository;
            _healthService = healthService;
        }

        [QueueTrigger("quanta-in")]
        public async Task ProcessInMessage(QuantaCashInMsg msg)
        {
            var logTask = _log.WriteInfoAsync(nameof(QuantaQueueHandler), nameof(ProcessInMessage), msg.ToJson(),
                "Message received");

            try
            {
                _healthService.TraceMessageProcessingStarted();

                var walletCreds = await _walletCredentialsRepository.GetByQuantaContractAsync(msg.Contract);
                if (walletCreds == null)
                {
                    await _log.WriteWarningAsync(nameof(QuantaQueueHandler), nameof(ProcessInMessage), msg.ToJson(), "Wallet not found");

                    _healthService.TraceMessageProcessingFailed();

                    return;
                }

                await _paymentSystemsRawLog.RegisterEventAsync(PaymentSystemRawLogEvent.Create(CashInPaymentSystem.Quanta, "Msg received", msg.ToJson()));

                var txId = $"{msg.TransactionHash}_{msg.Contract}";

                var pt = await _paymentTransactionsRepository.TryCreateAsync(PaymentTransaction.Create(
                    txId, CashInPaymentSystem.Quanta, walletCreds.ClientId, msg.Amount,
                    LykkeConstants.QuantaAssetId, LykkeConstants.QuantaAssetId, null));

                if (pt == null)
                {
                    await _log.WriteWarningAsync(nameof(QuantaQueueHandler), nameof(ProcessInMessage), msg.ToJson(), "Transaction already handled");
                    
                    _healthService.TraceMessageProcessingFailed();

                    return;
                }

                var result = await _exchangeOperationsService.CashInAsync(walletCreds.ClientId, LykkeConstants.QuantaAssetId, msg.Amount);

                if (!result.IsOk())
                {
                    await _log.WriteWarningAsync(nameof(QuantaQueueHandler), msg.ToJson(), result.ToJson(), "ME error");

                    _healthService.TraceMessageProcessingFailed();

                    return;
                }

                await _paymentTransactionsRepository.SetAsOkAsync(pt.Id, msg.Amount, null);

                _healthService.TraceMessageProcessingCompleted();
            }
            catch (Exception ex)
            {
                await _log.WriteErrorAsync(nameof(QuantaQueueHandler), nameof(ProcessInMessage), msg.ToJson(), ex);

                _healthService.TraceMessageProcessingFailed();
            }
            finally
            {
                await logTask;
            }
        }
    }
}