using ChatBot.Core.Models;
using LanguageExt;

namespace ChatBot.Core.Interfaces
{
    public interface IUserService
    {
        Task<ChatUser?> GetUser(string id);
    }
}