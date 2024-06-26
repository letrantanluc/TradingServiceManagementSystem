﻿using Do_An_Chuyen_Nganh.Data;
using Do_An_Chuyen_Nganh.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace Do_An_Chuyen_Nganh.Hubs
{
    public class ChatHub :Hub
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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
                ReceiverID = receiverId,
                SenderUsername = senderUsername,  // Thêm SenderUsername vào Message
                ReceiverUsername = receiverUsername
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
            var user = await _userManager.FindByIdAsync(userId);
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

        public async Task<IEnumerable<string>> GetUsersWithMessages(string userId)
        {
            //var senderMessages = await _context.Messages.Where(m => m.SenderID == userId).Select(m => m.ReceiverID).Distinct().ToListAsync();
            //var receiverMessages = await _context.Messages.Where(m => m.ReceiverID == userId).Select(m => m.SenderID).Distinct().ToListAsync();
            //var allUsers = senderMessages.Concat(receiverMessages).Distinct();
            var senderUsers = await _context.Messages
                .Where(m => m.SenderID == userId)
                .Select(m => m.ReceiverID)
                .Distinct()
                .ToListAsync();

            var receiverUsers = await _context.Messages
                .Where(m => m.ReceiverID == userId)
                .Select(m => m.SenderID)
                .Distinct()
                .ToListAsync();

            var allUserIDs = senderUsers.Concat(receiverUsers).Distinct().ToList();
            //var allUserIDs = senderUserIDs.Concat(receiverUserIDs).Distinct();
            var allUsers = await _context.Users
                .Where(u => allUserIDs.Contains(u.Id))
                .ToListAsync();
            var usernames = new List<string>();
            await Clients.Caller.SendCoreAsync("ReceiveUsersWithMessages", new object[] { allUsers });
            return usernames;
        }
    }
}