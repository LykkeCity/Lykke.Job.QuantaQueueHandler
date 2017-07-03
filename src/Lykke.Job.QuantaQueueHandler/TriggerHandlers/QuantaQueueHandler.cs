using System;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Job.QuantaQueueHandler.Contract;
using Lykke.Job.QuantaQueueHandler.Core;
using Lykke.Job.QuantaQueueHandler.Core.Domain.BitCoin;
using Lykke.Job.QuantaQueueHandler.Core.Domain.PaymentSystems;
using Lykke.Job.QuantaQueueHandler.Services.Exchange;
using Lykke.JobTriggers.Triggers.Attributes;

namespace Lykke.Job.QuantaQueueHandler.TriggerHandlers
{
    public class QuantaQueueHandler
    {
        private readonly IWalletCredentialsRepository _walletCredentialsRepository;
        private readonly ILog _log;
        private readonly ExchangeOperationsService _exchangeOperationsService;
        private readonly IPaymentSystemsRawLog _paymentSystemsRawLog;
        private readonly IPaymentTransactionsRepository _paymentTransactionsRepository;

        public QuantaQueueHandler(IWalletCredentialsRepository walletCredentialsRepository, ILog log,
            ExchangeOperationsService exchangeOperationsService, IPaymentSystemsRawLog paymentSystemsRawLog,
            IPaymentTransactionsRepository paymentTransactionsRepository)
        {
            _walletCredentialsRepository = walletCredentialsRepository;
            _log = log;
            _exchangeOperationsService = exchangeOperationsService;
            _paymentSystemsRawLog = paymentSystemsRawLog;
            _paymentTransactionsRepository = paymentTransactionsRepository;
        }

        [QueueTrigger("quanta-in")]
        public async Task ProcessInMessage(QuantaCashInMsg msg)
        {
            var logTask = _log.WriteInfoAsync(nameof(QuantaQueueHandler), nameof(ProcessInMessage), msg.ToJson(),
                "Message received");

            try
            {
                var walletCreds = await _walletCredentialsRepository.GetByQuantaContractAsync(msg.Contract);
                if (walletCreds == null)
                {
                    await
                        _log.WriteWarningAsync(nameof(QuantaQueueHandler), nameof(ProcessInMessage), msg.ToJson(),
                            "Wallet not found");
                    return;
                }

                await
                    _paymentSystemsRawLog.RegisterEventAsync(
                        PaymentSystemRawLogEvent.Create(CashInPaymentSystem.Quanta, "Msg received",
                            msg.ToJson()));

                var txId = $"{msg.TransactionHash}_{msg.Contract}";

                var pt = await _paymentTransactionsRepository.TryCreateAsync(PaymentTransaction.Create(
                    txId, CashInPaymentSystem.Quanta, walletCreds.ClientId, msg.Amount,
                    LykkeConstants.QuantaAssetId, LykkeConstants.QuantaAssetId, null));

                if (pt == null)
                {
                    await
                        _log.WriteWarningAsync(nameof(QuantaQueueHandler), nameof(ProcessInMessage), msg.ToJson(),
                            "Transaction already handled");
                    //return if was handled previously
                    return;
                }

                var result = await
                    _exchangeOperationsService.IssueAsync(walletCreds.ClientId, LykkeConstants.QuantaAssetId,
                        msg.Amount);

                if (!result.IsOk())
                {
                    await
                        _log.WriteWarningAsync(nameof(QuantaQueueHandler), msg.ToJson(), result.ToJson(),
                            "ME error");
                    return;
                }

                await
                    _paymentTransactionsRepository.SetAsOkAsync(pt.Id, msg.Amount, null);
            }
            catch (Exception ex)
            {
                await _log.WriteErrorAsync(nameof(QuantaQueueHandler), nameof(ProcessInMessage), msg.ToJson(), ex);
            }
            finally
            {
                await logTask;
            }
        }
    }
}