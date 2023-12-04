using Do_An_Chuyen_Nganh.Models;
using Microsoft.AspNetCore.SignalR;

namespace Do_An_Chuyen_Nganh.Hubs
{
    public class ChatHub :Hub
    {
         public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
