using ChatBot.Core.Entities;
using ChatBot.Core.Interfaces;
using ChatBot.Core.Utils;
using LanguageExt;

namespace ChatBot.Pages.Services
{
    public class CommandService : ICommandService
    {
        private CommandInfo _commandInfo = new(string.Empty, string.Empty, string.Empty);

        private readonly List<string> _commands = new() { "/stock" };

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

            if (string.IsNullOrWhiteSpace(param))
                return Constants.ERROR_NULL_PARAMETER;

            return string.Empty;
        }

        public Option<CommandInfo> GetCommandInfos(string text)
        {
            string error = GetCommandError(text);
            if (string.IsNullOrEmpty(error))
            {
                //check if val
                string[] splitter = text.Split("=");
                string command = splitter[0];
                if (!_commands.Contains(command))
                    return Option<CommandInfo>.None;

                string parameter = splitter[1];
                _commandInfo = _commandInfo with { Command = command, Parameter = parameter };

            }
            else
                _commandInfo = _commandInfo with { Error = error };
            return _commandInfo;
        }

        public bool IsCommand(string text)
        {
            return text.StartsWith("/");
        }
    }
}
