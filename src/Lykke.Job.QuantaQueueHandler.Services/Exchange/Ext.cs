using Lykke.MatchingEngine.Connector.Abstractions.Models;

namespace Lykke.Job.QuantaQueueHandler.Services.Exchange
{
    public static class Ext
    {
        public static ResultModel ToDomainModel(this MeResponseModel meModel)
        {
            return new ResultModel
            {
                Code = (int)meModel.Status,
                Message = meModel.Message
            };
        }

        public static bool IsOk(this ResultModel model)
        {
            return model.Code == 0;
        }
    }
}