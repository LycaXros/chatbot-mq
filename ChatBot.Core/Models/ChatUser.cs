using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ChatBot.Core.Models
{
    public class ChatUser : IdentityUser
    {

        [Required]
        [Display(Name = "User Display Name")]
        public string DisplayName { get; set; }
        public virtual ICollection<Message> Messages { get; set; } = new HashSet<Message>();
    }
}
