using System.ComponentModel.DataAnnotations;

namespace ChatBot.Core.Models
{
    public sealed class Message
    {
        public Message() { }
        public Message(string text, ChatUser? sender)
        {
            this.Text = text;
            if (sender is not null)
                this.Sender = sender;
        }

        [Key] public string Id { get; set; } = default!;

        public DateTimeOffset SentAt { get; set; } = DateTimeOffset.Now;

        [Required]
        public string Text { get; set; } = default!;

        public string UserId { get; set; } = default!;

        public ChatUser Sender { get; set; } = default!;
    }
}
