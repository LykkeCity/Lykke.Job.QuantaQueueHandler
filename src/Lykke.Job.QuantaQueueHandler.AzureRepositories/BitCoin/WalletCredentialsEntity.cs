using Lykke.Job.QuantaQueueHandler.Core.Domain.BitCoin;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Job.QuantaQueueHandler.AzureRepositories.BitCoin
{
    public class WalletCredentialsEntity : TableEntity, IWalletCredentials
    {
        public static class ByClientId
        {
            public static string GeneratePartitionKey()
            {
                return "Wallet";
            }

            public static string GenerateRowKey(string clientId)
            {
                return clientId;
            }

            public static WalletCredentialsEntity CreateNew(IWalletCredentials src)
            {
                var entity = Create(src);
                entity.PartitionKey = GeneratePartitionKey();
                entity.RowKey = GenerateRowKey(src.ClientId);
                return entity;
            }
        }

        public static class ByQuantaContract
        {
            public static string GeneratePartitionKey()
            {
                return "QuantaContract";
            }

            public static string GenerateRowKey(string contract)
            {
                return contract;
            }

            public static WalletCredentialsEntity CreateNew(IWalletCredentials src)
            {
                var entity = Create(src);
                entity.PartitionKey = GeneratePartitionKey();
                entity.RowKey = GenerateRowKey(src.QuantaContract);
                return entity;
            }
        }

        public static WalletCredentialsEntity Create(IWalletCredentials src)
        {
            return new WalletCredentialsEntity
            {
                ClientId = src.ClientId,
                PrivateKey = src.PrivateKey,
                Address = src.Address,
                MultiSig = src.MultiSig,
                ColoredMultiSig = src.ColoredMultiSig,
                PreventTxDetection = src.PreventTxDetection,
                EncodedPrivateKey = src.EncodedPrivateKey,
                PublicKey = src.PublicKey,
                BtcConvertionWalletPrivateKey = src.BtcConvertionWalletPrivateKey,
                BtcConvertionWalletAddress = src.BtcConvertionWalletAddress,
                EthConversionWalletAddress = src.EthConversionWalletAddress,
                EthAddress = src.EthAddress,
                EthPublicKey = src.EthPublicKey,
                SolarCoinWalletAddress = src.SolarCoinWalletAddress,
                ChronoBankContract = src.ChronoBankContract,
                QuantaContract = src.QuantaContract
            };
        }

        public string ClientId { get; set; }
        public string Address { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public string MultiSig { get; set; }
        public string ColoredMultiSig { get; set; }
        public bool PreventTxDetection { get; set; }
        public string EncodedPrivateKey { get; set; }
        public string BtcConvertionWalletPrivateKey { get; set; }
        public string BtcConvertionWalletAddress { get; set; }
        public string EthConversionWalletAddress { get; set; }
        public string EthAddress { get; set; }
        public string EthPublicKey { get; set; }
        public string SolarCoinWalletAddress { get; set; }
        public string ChronoBankContract { get; set; }
        public string QuantaContract { get; set; }
    }
}