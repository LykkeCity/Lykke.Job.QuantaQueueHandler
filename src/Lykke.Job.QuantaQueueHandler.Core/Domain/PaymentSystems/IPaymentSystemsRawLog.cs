using System.Threading.Tasks;

namespace Lykke.Job.QuantaQueueHandler.Core.Domain.PaymentSystems
{
    public interface IPaymentSystemsRawLog
    {
        Task RegisterEventAsync(IPaymentSystemRawLogEvent evnt);

    }
}