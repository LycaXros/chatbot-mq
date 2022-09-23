using ChatBot.Core.Entities;
using ChatBot.Core.Interfaces;
using ChatBot.Core.Utils;

namespace ChatBot.Pages.Services
{
    public class CommandService : ICommandService
    {
        private CommandInfo _commandInfo = new CommandInfo(string.Empty, string.Empty, string.Empty);

        private List<string> _commands = new List<string>() { "/stock" };

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

        public CommandInfo? GetCommandInfos(string text)
        {
            string error = GetCommandError(text);
            if (string.IsNullOrEmpty(error))
            {
                //check if val
                string[] splitter = text.Split("=");
                string command = splitter[0];
                if (!_commands.Contains(command))
                    return null;

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
