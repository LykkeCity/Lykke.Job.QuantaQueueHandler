using System;
using Autofac;
using AzureStorage.Tables;
using AzureStorage.Tables.Templates.Index;
using Common;
using Common.Log;
using Lykke.Job.QuantaQueueHandler.AzureRepositories.BitCoin;
using Lykke.Job.QuantaQueueHandler.AzureRepositories.PaymentSystems;
using Lykke.Job.QuantaQueueHandler.Core;
using Lykke.Job.QuantaQueueHandler.Core.Domain.BitCoin;
using Lykke.Job.QuantaQueueHandler.Core.Domain.PaymentSystems;
using Lykke.Job.QuantaQueueHandler.Core.Services;
using Lykke.Job.QuantaQueueHandler.Services;
using Lykke.Job.QuantaQueueHandler.Services.Exchange;
using Lykke.MatchingEngine.Connector.Services;

namespace Lykke.Job.QuantaQueueHandler.Modules
{
    public class JobModule : Module
    {
        private readonly AppSettings.QuantaQueueHandlerSettings _settings;
        private readonly ILog _log;

        public JobModule(AppSettings.QuantaQueueHandlerSettings settings, ILog log)
        {
            _settings = settings;
            _log = log;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_settings)
                .SingleInstance();

            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();

            builder.RegisterInstance<IHealthService>(new HealthService(
                _settings.Health.MaxMessageProcessingDuration,
                _settings.Health.MaxMessageProcessingFailedInARow,
                _settings.Health.MaxMessageProcessingIdleDuration));

            // NOTE: You can implement your own poison queue notifier. See https://github.com/LykkeCity/JobTriggers/blob/master/readme.md
            // builder.Register<PoisionQueueNotifierImplementation>().As<IPoisionQueueNotifier>();

            var socketLog = new SocketLogDynamic(i => { },
                str => Console.WriteLine(DateTime.UtcNow.ToIsoDateTime() + ": " + str));

            builder.BindMeClient(_settings.MatchingEngine.IpEndpoint.GetClientIpEndPoint(), socketLog);

            RegisterAzureRepositories(builder, _settings.Db);
            RegisterServices(builder);
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<ExchangeOperationsService>().SingleInstance();
        }

        private void RegisterAzureRepositories(ContainerBuilder builder, AppSettings.DbSettings settings)
        {
            builder.RegisterInstance<IBitCoinTransactionsRepository>(
                new BitCoinTransactionsRepository(
                    new AzureTableStorage<BitCoinTransactionEntity>(settings.BitCoinQueueConnectionString, "BitCoinTransactions", _log)));

            builder.RegisterInstance<IWalletCredentialsRepository>(
                new WalletCredentialsRepository(
                    new AzureTableStorage<WalletCredentialsEntity>(settings.ClientPersonalInfoConnString, "WalletCredentials", _log)));


            builder.RegisterInstance<IPaymentTransactionsRepository>(
                new PaymentTransactionsRepository(
                    new AzureTableStorage<PaymentTransactionEntity>(settings.ClientPersonalInfoConnString, "PaymentTransactions", _log),
                    new AzureTableStorage<AzureMultiIndex>(settings.ClientPersonalInfoConnString, "PaymentTransactions", _log)));

            builder.RegisterInstance<IPaymentSystemsRawLog>(
                new PaymentSystemsRawLog(new AzureTableStorage<PaymentSystemRawLogEventEntity>(settings.LogsConnString, "PaymentSystemsLog", _log)));
        }
    }
}