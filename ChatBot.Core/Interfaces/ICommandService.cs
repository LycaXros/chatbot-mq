
using ChatBot.Core.Entities;
using LanguageExt;

namespace ChatBot.Core.Interfaces
{

    public interface ICommandService
    {
        string GetCommandError(string text);
        Option<CommandInfoError> GetCommandInfos(string text);
        bool IsCommand(string text);
    }
}
