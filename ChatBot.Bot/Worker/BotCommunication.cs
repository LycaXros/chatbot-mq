using ChatBot.Core.Entities;
using ChatBot.Core.RabbitMQ;
using ChatBot.Core.Utils;
using Microsoft.Extensions.Logging;

namespace ChatBot.Bot.Worker
{
    public class BotCommunication : RabbitService
    {

        private readonly HttpClient _client;
        private readonly ILogger<BotCommunication> _logger;
        private const string _url = "https://stooq.com/q/l/?s=stock&f=sd2t2ohlcv&h&e=csv";

        public BotCommunication(string connectionString, ILogger<BotCommunication> logger) : base(connectionString)
        {
            _client = new HttpClient();
            _logger = logger;
        }

        private async Task<string> GetStockMessage(string code)
        {
            try
            {
                _logger.LogInformation("Requesting Stock!!!");
                string response = await _client.GetStringAsync(_url.Replace("stock", code));

                /* Gets second line of csv */
                string[] lines = response.Split('\n');
                string secondLine = lines[1];

                /* Get properties */
                List<string> properties = secondLine.Split(",").ToList();
                Console.WriteLine($"Found Data: {properties}");

                string stockName = properties.First();
                properties.Reverse();
                string closePrice = properties[1];
                if (closePrice == "N/D")
                    throw new Exception("Not found!");

                return $"{stockName} quote is ${closePrice} per share";
            }
            catch (Exception ex)
            {
                return $"Error trying to get stock \"{code}\": " + ex.Message;
            }
        }

        public void WaitForStockCode()
        {
            Consume<string>(Constants.USERS_QUEUE_NAME, async (code) =>
            {
                string message = await GetStockMessage(code);
                BotMessageToUsers(message);
            });
        }

        public void BotMessageToUsers(string message)
        {
            ChatMessage chatMessage = new(DateTimeOffset.Now, message, Constants.BOT_NAME, Constants.BOT_NAME);

            _logger.LogInformation("Sending Chat Message: {msg}", chatMessage);
            Produce(Constants.BOT_QUEUE_NAME, chatMessage);
        }

    }
}
