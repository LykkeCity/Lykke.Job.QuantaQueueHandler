using System.Threading.Tasks;

namespace Lykke.Job.QuantaQueueHandler.Core.Domain.PaymentSystems
{
    public interface IPaymentTransactionsRepository
    {
        Task<IPaymentTransaction> TryCreateAsync(IPaymentTransaction paymentTransaction);

        Task<IPaymentTransaction> SetAsOkAsync(string id, double depositedAmount, double? rate);
    }
}