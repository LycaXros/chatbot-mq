using ChatBot.Core.Models;

namespace ChatBot.Core.Interfaces
{
    public interface IUserService
    {
        Task<ChatUser?> GetUser(string id);
    }
}