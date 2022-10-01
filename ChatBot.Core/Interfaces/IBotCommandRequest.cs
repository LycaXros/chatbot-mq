using ChatBot.Core.Entities;

namespace ChatBot.Core.Interfaces
{
    public interface IBotCommandRequest
    {
        void ExecuteCommand(CommandInfoError commandInfo);
    }
}