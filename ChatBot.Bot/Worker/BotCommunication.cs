using ChatBot.Core.Entities;
using ChatBot.Core.RabbitMQ;
using ChatBot.Core.Utils;
using Microsoft.Extensions.Logging;

namespace ChatBot.Bot.Worker
{
    /// <summary>
    /// Class Implementation for Reading the Queue of Requested Stocks
    /// Also is responsable of getting the HTTP Response of the Call for the Stock
    /// </summary>
    public class BotCommunication : RabbitService
    {

        private readonly HttpClient _client;
        private readonly ILogger<BotCommunication> _logger;
        private const string Url = "https://stooq.com/q/l/?s=stock&f=sd2t2ohlcv&h&e=csv";

        public BotCommunication(string connectionString, ILogger<BotCommunication> logger) : base(connectionString, logger)
        {
            _client = new HttpClient();
            _logger = logger;
        }

        /// <summary>
        /// Method for calling the HttpRequest, and managing the Response
        /// </summary>
        /// <param name="code">Stock Code to search</param>
        /// <returns>Formatted string with the stock information</returns>
        private async Task<string> GetStockMessage(string code)
        {
            try
            {
                _logger.LogInformation("Requesting Stock!!!");
                string response = await _client.GetStringAsync(Url.Replace("stock", code));

                _logger.LogInformation("Response of stock {stock} on {method} method : {data}",
                    code,
                    nameof(GetStockMessage),
                    response);
                
                var lines = response.Split('\n');
                var secondLine = lines[1];

                var properties = secondLine.Split(",").ToList();
                _logger.LogInformation("Found Data: {properties}", properties);

                string stockName = properties.First();
                properties.Reverse();
                string closePrice = properties[1];
                if (closePrice == "N/D")
                    throw new Exception("Not found!");

                return $"{stockName} quote is ${closePrice} per share";
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error trying to get stock \"{code}\": {message}", code, ex.Message );

                return $"Error trying to get stock \"{code}\": " + ex.Message;
            }
        }

        public void WaitForStockCode()
        {
            _logger.LogInformation($"Setting Consume Queue Method on function {nameof(WaitForStockCode)}");
            Consume<string>(Constants.USERS_QUEUE_NAME, ResolveStockCode);
        }

        private async void ResolveStockCode(string code)
        {
            var message = await GetStockMessage(code);
            BotMessageToUsers(message);
        }

        /// <summary>
        /// Function to send Messages to the Queue for the Bot User
        /// </summary>
        /// <param name="message">Message to Send</param>
        public void BotMessageToUsers(string message)
        {
            ChatMessage chatMessage = new(DateTimeOffset.Now, message, Constants.BOT_NAME, Constants.BOT_NAME);

            _logger.LogInformation("Sending Chat Message: {msg}", chatMessage);
            Produce(Constants.BOT_QUEUE_NAME, chatMessage);
        }

    }
}
