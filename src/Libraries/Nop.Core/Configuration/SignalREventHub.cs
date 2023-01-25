using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Nop.Core.Events;

namespace Nop.Core.Configuration
{
    public class SignalREventHub : Hub
    { 
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendEvent(PushEventModel pushEventModel)
        {
            await Clients.All.SendAsync("ReceiveEvent", pushEventModel);
        }
    }
}
