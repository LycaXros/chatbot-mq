using ChatBot.Core.Models;

namespace ChatBot.Core.Interfaces
{
    public interface IMessageService
    {
        Task<Message> AddMessage(Message message);

        Task<IList<Message>> GetLastMessages(int count = 50);
    }
}
