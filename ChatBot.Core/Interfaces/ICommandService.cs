
using ChatBot.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot.Core.Interfaces
{

    public interface ICommandService
    {
        string GetCommandError(string text);
        CommandInfo? GetCommandInfos(string text);
        bool IsCommand(string text);
    }
}
