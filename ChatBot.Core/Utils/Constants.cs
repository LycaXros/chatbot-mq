namespace ChatBot.Core.Utils
{
    public class Constants
    {

        public const string BOT_QUEUE_NAME = "BOT_COMMANDS_QUEUE";
        public const string USERS_QUEUE_NAME = "USERS_MESSAGES_QUEUE";
        public const string BOT_NAME = "StockBot";
        public const string RabbitConnection = "RabbitConnectionString";

        public const string ERROR_COMMAND_NOT_FOUND = "Command not found!";
        public const string ERROR_INVALID_FORMAT = "Invalid Format for Command";
        public const string ERROR_NULL_PARAMETER = "Parameter can not be null!";
        public const string ERROR_NULL_PARAMETER_INDICATOR = "Error! Indicator '=' is missing";

        public const string StockCommand = "/stock";

    }
}
