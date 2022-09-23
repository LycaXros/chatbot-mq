using ChatBot.Core.Interfaces;
using ChatBot.Core.Models;
using ChatBot.Pages.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Pages.Services
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext _context;
        public MessageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Message> AddMessage(Message message)
        {
            message.Id = (Guid.NewGuid()).ToString();
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<IList<Message>> GetLastMessages(int count = 50)
        {
            return await _context
                .Messages
                .Include(i => i.Sender)
                .OrderBy(o => o.SentAt)
                .Take(count)
                .ToListAsync();

        }
    }
}
