using Do_An_Chuyen_Nganh.Data;
using Do_An_Chuyen_Nganh.Models;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Do_An_Chuyen_Nganh.Hubs
{
    public class ChatHub :Hub
    {
        private readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task SendMessage(string receiverId, string message)
        {
            var senderId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var chatMessage = new Message
            {
                Text = message,
                SenderID = senderId,
                ReceiverID = receiverId
            };

            _context.Messages.Add(chatMessage);
            await _context.SaveChangesAsync();

            await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, message);
        }
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}