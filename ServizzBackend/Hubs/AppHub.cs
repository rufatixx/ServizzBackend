using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WoozzyBackend.Hubs
{
    public class AppHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}