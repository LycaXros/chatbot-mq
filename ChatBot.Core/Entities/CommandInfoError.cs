namespace ChatBot.Core.Entities
{
    public record CommandInfoError(string Command, string Error, string Parameter): CommandInfo(Command, Parameter);

    public record CommandInfo(string Command, string Parameter);

}
