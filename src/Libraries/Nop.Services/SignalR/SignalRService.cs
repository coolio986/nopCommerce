using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json.Linq;
using Nop.Core.Configuration;
using Nop.Core.Domain.SignalR;

namespace Nop.Services.SignalR
{
    public class SignalRService : ISignalRService
    {
        private readonly IHubContext<SignalREventHub> _hubContext,
        private readonly HubConnection? hubConnection;

        public SignalRService(IHubContext<SignalREventHub> hubContext)
        {
            _hubContext = hubContext;

            hubConnection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5154/nopSqlEventServer").Build();

            hubConnection.On<Audit>("orderEvent", OnOrderChange);
            hubConnection.StartAsync();

        }
        public async Task OnOrderChange(Audit audit)
        {
            if(_hubContext != null && audit != null && audit.TriggerType == "INSERT")
            {
                await _hubContext.Clients.All.SendAsync("RecieveEvent", audit);
            }
        }
    }
}
