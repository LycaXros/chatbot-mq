using System.Globalization;
using ChatBot.Core.Entities;
using ChatBot.Core.RabbitMQ;
using ChatBot.Core.Utils;
using CsvHelper;
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
        private readonly string _stockApiUrl;
        private readonly ILogger<BotCommunication> _logger;

        public BotCommunication(string connectionString, string stockApiUrl, ILogger<BotCommunication> logger) : base(connectionString, logger)
        {
            _client = new HttpClient();
            _stockApiUrl = stockApiUrl;
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
                var url = $"{_stockApiUrl}?s={code}&f=sd2t2ohlcv&h&e=csv";
                _logger.LogInformation("Requesting Stock!!!");
                var response = await _client.GetStreamAsync(url);

                using var reader = new StreamReader(response);
                using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                if (!await csvReader.ReadAsync()) throw new Exception("Could Not Read the File");

                var item = csvReader.GetRecord<StockData>();

                var raw = csvReader.GetRecord<object>();


                _logger.LogInformation("Response of stock {stock} on {method} method : {data}",
                    code,
                    nameof(GetStockMessage),
                    raw);

                if (item is not null && item.Close != "N/D")
                {

                    return $"{item.Symbol} quote is ${item.Close} per share";
                }

                throw new Exception("Not found!");

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error trying to get stock \"{code}\": {message}", code, ex.Message);

                return $"Error trying to get stock \"{code}\": " + ex.Message;
            }
        }

        public void WaitForStockCode()
        {
            _logger.LogInformation($"Setting Consume Queue Method on function {nameof(WaitForStockCode)}");
            Consume<CommandInfo>(Constants.BOT_QUEUE_NAME, ResolveStockCode);
        }

        private async void ResolveStockCode(CommandInfo command)
        {
            if (command.Command == Constants.StockCommand)
            {
                var message = await GetStockMessage(command.Parameter);
                BotMessageToUsers(message);
            }
            else
            {
                _logger.LogInformation("CommandInfo {command} is not a Stock Command", command);
            }
        }

        /// <summary>
        /// Function to send Messages to the Queue for the Bot User
        /// </summary>
        /// <param name="message">Message to Send</param>
        public void BotMessageToUsers(string message)
        {
            ChatMessage chatMessage = new(DateTimeOffset.Now, message, Constants.BOT_NAME, Constants.BOT_NAME);

            _logger.LogInformation("Sending Chat Message: {msg}", chatMessage);
            Produce(Constants.USERS_QUEUE_NAME, chatMessage);
        }

    }
}
