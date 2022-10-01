using ChatBot.Core.Entities;
using ChatBot.Core.Interfaces;
using ChatBot.Core.Utils;
using LanguageExt;

namespace ChatBot.Pages.Services
{
    public class CommandService : ICommandService
    {

        private readonly List<string> _commands = new() { Constants.StockCommand };

        public string GetCommandError(string text)
        {
            if (!text.Contains('='))
                return Constants.ERROR_NULL_PARAMETER_INDICATOR;

            string[] splitter = text.Split("=");
            string command = splitter[0];
            string param = splitter[1];
            if (!command.StartsWith("/"))
                return Constants.ERROR_INVALID_FORMAT;

            if (!_commands.Contains(command))
                return $"'{command}' " + Constants.ERROR_COMMAND_NOT_FOUND;

            return string.IsNullOrWhiteSpace(param) ? Constants.ERROR_NULL_PARAMETER : string.Empty;
        }

        public Option<CommandInfoError> GetCommandInfos(string text)
        {
            CommandInfoError commandInfoError = new(string.Empty, string.Empty, string.Empty);
            string error = GetCommandError(text);
            if (string.IsNullOrEmpty(error))
            {
                //check if val
                string[] splitter = text.Split("=");
                string command = splitter[0];
                if (!_commands.Contains(command))
                    return Option<CommandInfoError>.None;

                string parameter = splitter[1];
                commandInfoError = commandInfoError with { Command = command, Parameter = parameter };

            }
            else
                commandInfoError = commandInfoError with { Error = error };
            return commandInfoError;
        }

        public bool IsCommand(string text)
        {
            return text.StartsWith("/");
        }
    }
}
