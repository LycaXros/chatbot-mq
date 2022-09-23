using System.ComponentModel.DataAnnotations;

namespace ChatBot.Core.Models
{
    public class Message
    {
        public Message() { }
        public Message(string text, ChatUser sender)
        {
            this.Text = text;
            this.Sender = sender;
        }

        [Key]
        public string Id { get; set; }

        public DateTimeOffset SentAt { get; set; } = DateTimeOffset.Now;

        [Required]
        public string Text { get; set; }

        public string UserId { get; set; }

        public virtual ChatUser Sender { get; set; }
    }
}
