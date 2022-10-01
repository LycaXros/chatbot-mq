using ChatBot.Core.Entities;
using ChatBot.Core.Interfaces;
using ChatBot.Core.RabbitMQ;
using ChatBot.Core.Utils;

namespace ChatBot.Pages.Services
{
    public class BotCommandRequest : RabbitService, IBotCommandRequest
    {
        private readonly ILogger<BotCommandRequest> _logger;
        public BotCommandRequest(IConfiguration configuration, ILogger<BotCommandRequest> logger)
            : base(Helper.GetConnection(configuration, Constants.RabbitConnection), logger)
        {
            _logger = logger;
        }

        public void ExecuteCommand(CommandInfoError commandInfo)
        {
            CommandInfo command = commandInfo;
            _logger.LogInformation("Sending to queue the request for {stock}", command);
            Produce(Constants.BOT_QUEUE_NAME, command);
        }
    }
}
