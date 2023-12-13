using Do_An_Chuyen_Nganh.Data;
using Do_An_Chuyen_Nganh.Hubs;
using Do_An_Chuyen_Nganh.Models;
using Do_An_Chuyen_Nganh.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Do_An_Chuyen_Nganh.Controllers
{
    public class MessageController : BaseController<Message>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ChatHub> _chatHubContext;


        public MessageController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHubContext<ChatHub> chatHubContext) : base(context)
        {
            _context = context;
            _chatHubContext = chatHubContext;
        }
        public async Task<ActionResult> Chat()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Send a request to the SignalR hub to get the users with messages
            await _chatHubContext.Clients.All.SendCoreAsync("GetUsersWithMessages", new object[] { userId });

            // You may want to retrieve the list of users here if needed
            var usersWithMessages = await _context.Messages
                .Where(m => m.SenderID == userId || m.ReceiverID == userId)
                .Select(m => m.SenderID == userId ? m.ReceiverID : m.SenderID)
                .Distinct()
                .ToListAsync();

            return View(usersWithMessages);
        }
    }
}
