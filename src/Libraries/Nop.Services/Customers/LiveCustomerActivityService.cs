using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.SignalR;
using Nop.Core.Configuration;
using Nop.Core.Domain.Customers;

namespace Nop.Services.Customers
{
    public class LiveCustomerActivityService : ILiveCustomerActivityService
    {
        private readonly CustomerSettings _customerSettings;
        private readonly ICustomerService _customerService;
        private readonly IHubContext<SignalREventHub> _hubContext;
        private readonly Timer _timer;

        public LiveCustomerActivityService(CustomerSettings customerSettings, 
            IHubContext<SignalREventHub> hubContext, 
            ICustomerService customerService)
        {
            _customerSettings = customerSettings;
            _customerService = customerService;
            _hubContext = hubContext;

            _timer = new Timer(15000);
            _timer.Enabled = true;
            _timer.Elapsed += CustomerTimerCallback;
            _customerService = customerService;
        }
        private void CustomerTimerCallback(object sender, ElapsedEventArgs e)
        {
            SendDataToDashboard();
        }
        
        private async void SendDataToDashboard()
        {
            await PrepareOnlineCustomerListModelAsync();
        }

        public async Task PrepareOnlineCustomerListModelAsync()
        {
            //get parameters to filter customers
            var lastActivityFrom = DateTime.UtcNow.AddMinutes(-5);

            //get online customers
            var customers = await _customerService.GetOnlineCustomersAsync(customerRoleIds: null,
                 lastActivityFromUtc: lastActivityFrom);

            await _hubContext.Clients.All.SendAsync("LiveCustomerUpdate", customers.Count());
        }

        public async Task UpdateVisitorCounter()
        {
            await PrepareOnlineCustomerListModelAsync();
        }
    }
}
