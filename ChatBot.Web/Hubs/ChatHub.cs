using ChatBot.Core.Entities;
using ChatBot.Core.Interfaces;
using ChatBot.Core.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatBot.Web.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ICommandService _command;

        public ChatHub(ICommandService command)
        {
            _command = command;
        }


        public async void SendAll(ChatMessage chatMessage)
        {

            if (_command.IsCommand(chatMessage.Text))
            {
                CommandInfo infos = _command.GetCommandInfos(chatMessage.Text);

                await Broadcast(chatMessage);

                if (infos.Error != null)
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

                //string userId = chatMessage.UserID;
                //ChatUser chatUser = _userService.GetUser(userId);
                //Message message = new Message(chatMessage.Text, chatUser);
                //_messageService.AddMessage(message);
                //chatMessage = chatMessage with { SentAt = message.SentAt };
                //await Broadcast(chatMessage);
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
