using Do_An_Chuyen_Nganh.Data;
using Do_An_Chuyen_Nganh.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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
            Console.WriteLine($"SendMessage called: {Context.ConnectionId}");

            var senderId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var senderUsername = await GetUsernameFromUserId(senderId);
            var receiverUsername = await GetUsernameFromUserId(receiverId);

            var chatMessage = new Message
            {
                Text = message,
                SenderID = senderId,
                ReceiverID = receiverId
            };

            _context.Messages.Add(chatMessage);
            await _context.SaveChangesAsync();

            await Clients.Groups(receiverId).SendAsync("ReceiveMessage", senderUsername, message);
        }
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        private async Task<string> GetUsernameFromUserId(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user != null ? user.UserName : "Unknown User";
        }
        public async Task<IEnumerable<Message>> GetMessages(string senderId, string receiverId)
        {
            var messages = await _context.Messages
                .Where(m => (m.SenderID == senderId && m.ReceiverID == receiverId) ||
                            (m.SenderID == receiverId && m.ReceiverID == senderId))
                .OrderBy(m => m.When)
                .ToListAsync();

            return messages;
        }
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }


    }
}