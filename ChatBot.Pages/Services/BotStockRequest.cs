using ChatBot.Core.Interfaces;
using ChatBot.Core.RabbitMQ;
using ChatBot.Core.Utils;

namespace ChatBot.Pages.Services
{
    public class BotStockRequest : RabbitService, IBotStockRequest
    {
        private readonly ILogger<BotStockRequest> _logger;
        public BotStockRequest(IConfiguration configuration, ILogger<BotStockRequest> logger)
            : base(Helper.GetConnection(configuration, Constants.RabbitConnection), logger)
        {
            _logger = logger;
        }

        public void SearchStock(string stockCode)
        {
            _logger.LogInformation("Sending to queue the request for {stock}", stockCode);
            Produce(Constants.USERS_QUEUE_NAME, stockCode);
        }
    }
}
