using ChatBot.Core.Entities;
using ChatBot.Core.Interfaces;
using ChatBot.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChatBot.Pages.Pages
{
    public class ChatRoomModel : PageModel
    {
        private readonly ILogger<ChatRoomModel> _logger;
        private readonly UserManager<ChatUser> _userManager;
        private readonly IMessageService _ms;

        public IList<ChatMessage> Messages;


        public ChatRoomModel(IMessageService ms, ILogger<ChatRoomModel> logger, UserManager<ChatUser> userManager)
        {
            _ms = ms;
            _logger = logger;
            _userManager = userManager;
            Messages = new List<ChatMessage>();
        }

        public async Task OnGetAsync()
        {
            var loggedUser = await _userManager.GetUserAsync(User);
            ViewData["DisplayName"] = loggedUser.DisplayName;
            ViewData["UserId"] = loggedUser.Id;
            var model = new List<ChatMessage>();

            var data = await _ms.GetLastMessages();
            if (data is not null && data.Count > 0)
            {
                Messages = data.Select(x => new ChatMessage(x.SentAt, x.Text, x.Sender.UserName, x.Sender.Id)).ToList();
            }
        }
    }
}
