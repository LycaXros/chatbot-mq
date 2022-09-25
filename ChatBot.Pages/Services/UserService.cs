using ChatBot.Core.Interfaces;
using ChatBot.Core.Models;
using ChatBot.Pages.Data;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Pages.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ChatUser?> GetUser(string id)
        {
            return await _context.ChatUsers.FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}
