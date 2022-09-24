using ChatBot.Core.Interfaces;
using ChatBot.Core.RabbitMQ;
using ChatBot.Core.Utils;

namespace ChatBot.Pages.Services
{
    public class BotStockRequest : RabbitService, IBotStockRequest
    {
        public BotStockRequest(IConfiguration configuration)
            : base(Helper.GetConnection(configuration, Constants.RabbitConnection))
        {

        }

        public void SearchStock(string stockCode)
        {
            Produce(Constants.USERS_QUEUE_NAME, stockCode);
        }
    }
}
