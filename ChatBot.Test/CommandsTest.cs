using ChatBot.Core.Interfaces;
using ChatBot.Core.Utils;
using ChatBot.Pages.Services;
using Xunit;

namespace ChatBot.Test
{
    public class CommandsTest
    {
        private readonly ICommandService _commandService = new CommandService();

        [Fact]
        public void Command_IsNot_A_Command()
        {
            string command = "Not a Command";

            var isCommand = _commandService.IsCommand(command);

            Assert.False(isCommand);

        }

        [Fact]
        public void Command_Is_A_Command()
        {
            string command = "/command";

            var isCommand = _commandService.IsCommand(command);

            Assert.True(isCommand);

        }

        [Fact]
        public void Command_Is_MissingIndicator_Command()
        {
            string command = "/command";

            var commandInfo = _commandService.GetCommandError(command);


            Assert.Equal(commandInfo, Constants.ERROR_NULL_PARAMETER_INDICATOR);

        }

        [Fact]
        public void Command_Is_NotFoundCommand_Command()
        {
            string command = "/command=";
            string msg = $"'{command.Replace("=","")}' {Constants.ERROR_COMMAND_NOT_FOUND}";

            var commandInfo = _commandService.GetCommandError(command);


            Assert.Equal(commandInfo, msg);

        }

        [Fact]
        public void Command_Is_NullParameter_Command()
        {
            string command = "/stock=";
            //$"'{command}' " + Constants.ERROR_COMMAND_NOT_FOUND;

            var commandInfo = _commandService.GetCommandError(command);


            Assert.Equal(commandInfo, Constants.ERROR_NULL_PARAMETER);

        }
        [Fact]
        public void Command_Is_InvalidFormat_Command()
        {
            string command = "command=bc";

            var commandInfo = _commandService.GetCommandError(command);


            Assert.Equal(commandInfo, Constants.ERROR_INVALID_FORMAT);

        }

        [Fact]
        public void Command_Is_Valid_Command()
        {
            string command = "/stock=btc.v";

            var commandInfo = _commandService.GetCommandError(command);


            Assert.Equal(commandInfo, string.Empty);

        }
    }
}