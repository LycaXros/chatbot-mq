using ChatBot.Core.Entities;
using ChatBot.Core.Interfaces;
using ChatBot.Core.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatBot.Web.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ICommandService _command;
        private readonly IMessageService _ms;
        private readonly IUserService _userService;
        private readonly IBotStockRequest _stockRequest;
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(ICommandService command, IMessageService ms, IUserService userService, IBotStockRequest stockRequest, ILogger<ChatHub> logger)
        {
            _command = command;
            _ms = ms;
            _userService = userService;
            _stockRequest = stockRequest;
            _logger = logger;
        }


        public async Task SendAll(ChatMessage chatMessage)
        {

            if (_command.IsCommand(chatMessage.Text))
            {
                var infos = _command.GetCommandInfos(chatMessage.Text);

                await Broadcast(chatMessage);

                if (infos is null)
                {
                    _logger.LogInformation("Information Invalid Command : {CommandText}", chatMessage.Text);
                    return;
                }

                if (!string.IsNullOrEmpty(infos.Error))
                {
                    await Broadcast(AdminMessage(infos.Error));
                }
                else
                {
                    _stockRequest.SearchStock(infos.Parameter);
                }
            }
            else
            {

                string userId = chatMessage.UserID;
                var chatUser = await _userService.GetUser(userId);

                Message message = new(chatMessage.Text, chatUser);
                await _ms.AddMessage(message);
                chatMessage = chatMessage with { SentAt = message.SentAt, UserName = chatUser?.DisplayName ?? chatMessage.UserName };
                await Broadcast(chatMessage);
            }
        }

        private static ChatMessage AdminMessage(string text)
        {
            return new ChatMessage(DateTimeOffset.Now, text, "Administrator", "ADMIN");
        }

        private async Task Broadcast(ChatMessage chatMessage)
        {
            _logger.LogInformation("Broadcasting message : {chatMessage}", chatMessage);
            await Clients.All.SendAsync("receive", chatMessage);
        }
    }
}
