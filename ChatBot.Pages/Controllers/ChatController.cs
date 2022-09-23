using ChatBot.Core.Entities;
using ChatBot.Core.Interfaces;
using ChatBot.Core.Models;
using ChatBot.Pages.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ChatBot.Pages.Controllers
{
    public class ChatController : Controller
    {

        private readonly ILogger<ChatController> _logger;
        private readonly UserManager<ChatUser> _userManager;
        private readonly IMessageService _ms;

        public ChatController(ILogger<ChatController> logger, UserManager<ChatUser> userManager, IMessageService messageService)
        {
            _logger = logger;
            _userManager = userManager;
            _ms = messageService;
        }


        [Authorize]
        public async Task<IActionResult> Index()
        {
            var loggedUser = await _userManager.GetUserAsync(User);
            ViewBag.DisplayName = loggedUser.DisplayName;
            ViewBag.UserId = loggedUser.Id;
            var model = new List<ChatMessage>();

            var data =  await _ms.GetLastMessages();
            if (data is not null && data.Count > 0) {
                model = data.Select(x => new ChatMessage(x.SentAt, x.Text, x.Sender.UserName, x.Sender.Id)).ToList();
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
