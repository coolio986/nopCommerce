using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.SignalR;
using Nop.Core;
using Nop.Core.Configuration;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Services.Helpers;
using Nop.Services.Orders;

namespace Nop.Services.Customers
{
    public class LiveCustomerActivityService : ILiveCustomerActivityService
    {
        private readonly CustomerSettings _customerSettings;
        private readonly ICustomerService _customerService;
        private readonly IWorkContext _workContext;
        private readonly IOrderReportService _orderReportService;
        private readonly IHubContext<SignalREventHub> _hubContext;
        private readonly Timer _timer;

        public LiveCustomerActivityService(CustomerSettings customerSettings, 
            IHubContext<SignalREventHub> hubContext, 
            ICustomerService customerService,
            IWorkContext workContext,
            IOrderReportService orderReportService)
        {
            _customerSettings = customerSettings;
            _customerService = customerService;
            _workContext = workContext;
            _orderReportService = orderReportService;
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

            var totalSessionCount = (await _customerService.GetTotalSessionsAsync(DateTime.UtcNow, DateTime.Today)).Count;

            var salesSummary = await GetSalesSummaryReportAsync();
            if(salesSummary != null && salesSummary.Count > 0)
            {
                await _hubContext.Clients.All.SendAsync("TotalDailySales", salesSummary[0].ProfitStr);
                await _hubContext.Clients.All.SendAsync("TotalDailyOrders", salesSummary[0].NumberOfOrders);
            }

            await _hubContext.Clients.All.SendAsync("TotalSessionCount", totalSessionCount);

            await _hubContext.Clients.All.SendAsync("VisitorCount", customers.Count());
        }

        public async Task<IPagedList<SalesSummaryReportLine>> GetSalesSummaryReportAsync()
        {
            //get parameters to filter orders
            OrderStatus? orderStatus = null;
            PaymentStatus? paymentStatus = null;

            var currentVendor = await _workContext.GetCurrentVendorAsync();

            var startDateValue = DateTime.Today;
            var endDateValue = DateTime.UtcNow;

            //get sales summary
            var salesSummary = await _orderReportService.SalesSummaryReportAsync(
                createdFromUtc: startDateValue,
                createdToUtc: endDateValue,
                os: orderStatus,
                ps: paymentStatus,
                billingCountryId: 0,
                groupBy: 0,
                categoryId: 0,
                productId: 0,
                manufacturerId: 0,
                vendorId: currentVendor?.Id ?? 0,
                storeId: 0,
                pageIndex: 0);

            return salesSummary;
        }

        public async Task KickStartLiveUpdates()
        {
            await PrepareOnlineCustomerListModelAsync();
        }
    }
}
