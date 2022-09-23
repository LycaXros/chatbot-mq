using Microsoft.AspNetCore.Identity;

namespace ChatBot.Core.Models
{
    public class ChatUser : IdentityUser
    {
        public virtual ICollection<Message> Messages { get; set; } = new HashSet<Message>();
    }
}
