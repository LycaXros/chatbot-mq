using ChatBot.Core.Entities;
using ChatBot.Core.RabbitMQ;
using ChatBot.Core.Utils;
using ChatBot.Web.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ChatBot.Pages.Services
{
    public class BotResponseCommunication : BackgroundService
    {

        private readonly IHubContext<ChatHub> _hubContext;
        private readonly RabbitService _service;
        public BotResponseCommunication(IConfiguration configuration, IHubContext<ChatHub> hubContext) 
        {
            _hubContext = hubContext;
            _service = new RabbitService(Helper.GetConnection(configuration, Constants.RabbitConnection));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

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
