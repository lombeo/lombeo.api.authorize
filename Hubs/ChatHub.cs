using Microsoft.AspNetCore.SignalR;

namespace Lombeo.Api.Authorize.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string sender, string receiver, string message)
        {
            await Clients.User(receiver).SendAsync("ReceiveMessage", sender, message);
        }
    }
}
