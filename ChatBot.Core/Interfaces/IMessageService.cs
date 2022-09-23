using ChatBot.Core.Models;

namespace ChatBot.Core.Interfaces
{
    public interface IMessageService
    {
        Message AddMessage(Message message);

        List<Message> GetLastMessages(int count = 50);
    }
}
