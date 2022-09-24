
using ChatBot.Core.Entities;

namespace ChatBot.Core.Interfaces
{

    public interface ICommandService
    {
        string GetCommandError(string text);
        CommandInfo? GetCommandInfos(string text);
        bool IsCommand(string text);
    }
}
