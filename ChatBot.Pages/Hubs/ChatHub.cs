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

        public ChatHub(ICommandService command, IMessageService ms, IUserService userService)
        {
            _command = command;
            _ms = ms;
            _userService = userService;
        }


        public async Task SendAll(ChatMessage chatMessage)
        {

            if (_command.IsCommand(chatMessage.Text))
            {
                var infos = _command.GetCommandInfos(chatMessage.Text);

                await Broadcast(chatMessage);

                if (infos is not null && !string.IsNullOrEmpty(infos.Error))
                {
                    await Broadcast(AdminMessage(infos.Error));
                }
                else
                {
                    //_userBotQueueProducer.SearchStock(infos.Parameter);
                }
            }
            else
            {

                string userId = chatMessage.UserID;
                var chatUser = await _userService.GetUser(userId);

                Message message = new(chatMessage.Text, chatUser);
                await _ms.AddMessage(message);
                chatMessage = chatMessage with { SentAt = message.SentAt };
                await Broadcast(chatMessage);
            }
        }

        private ChatMessage AdminMessage(string text)
        {
            return new ChatMessage(DateTimeOffset.Now, text, "Administrator", "ADMIN" );
        }

        private async Task Broadcast(ChatMessage chatMessage)
        {
            await Clients.All.SendAsync("receive", chatMessage);
        }
    }
}
