using ChatBot.Core.Entities;
using ChatBot.Core.RabbitMQ;
using ChatBot.Core.Utils;
using ChatBot.Pages.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ChatBot.Pages.Services
{
    public class BotResponseCommunication : BackgroundService
    {
        private readonly ILogger<BotResponseCommunication> _logger;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly RabbitService _service;
        public BotResponseCommunication(IConfiguration configuration, IHubContext<ChatHub> hubContext, ILoggerFactory loggerF) 
        {
            _hubContext = hubContext;
            _logger = loggerF.CreateLogger<BotResponseCommunication>();
            _service = new RabbitService(Helper.GetConnection(configuration, Constants.RabbitConnection), loggerF.CreateLogger<RabbitService>());
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            _logger.LogInformation("Waiting for Bot Response");
            WaitForBotResponse();
            return Task.CompletedTask;
        }

        private void WaitForBotResponse()
        {
            _service.Consume<ChatMessage>(Constants.BOT_QUEUE_NAME, async (botMsg) =>
            {
                await _hubContext.Clients.All.SendAsync("receive", botMsg);
            });
        }

        public override void Dispose()
        {
            _service.Dispose();
            base.Dispose();
        }
    }
}
