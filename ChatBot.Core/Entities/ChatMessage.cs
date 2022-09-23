using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot.Core.Entities
{
    public record ChatMessage(DateTimeOffset SentAt, string Text, string UserName, string UserID);
}
