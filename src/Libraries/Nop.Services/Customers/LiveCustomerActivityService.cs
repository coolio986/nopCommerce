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
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly Timer _timer;

        private readonly TimeZoneInfo _timeZone;

        public LiveCustomerActivityService(CustomerSettings customerSettings, 
            IHubContext<SignalREventHub> hubContext, 
            ICustomerService customerService,
            IWorkContext workContext,
            IOrderReportService orderReportService,
            IDateTimeHelper dateTimeHelper)
        {
            _customerSettings = customerSettings;
            _customerService = customerService;
            _workContext = workContext;
            _orderReportService = orderReportService;
            _hubContext = hubContext;
            _dateTimeHelper = dateTimeHelper;

            _timeZone = dateTimeHelper.DefaultStoreTimeZone;



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

            List<string> alreadyUsedIPs = new List<string>();

            var startDateValue = DateTime.Today;

            //hack... trying to fix issues between docker datetime and windows
            var endDateValue = DateTime.UtcNow;
            if (endDateValue.AddHours(_timeZone.BaseUtcOffset.Hours) < DateTime.Today)
                startDateValue = startDateValue.AddDays(-1);


            var customerSessions = await _customerService.GetTotalSessionsAsync(endDateValue, startDateValue);
            
            foreach(var customerSession in customerSessions.ToList())
            {
                if (alreadyUsedIPs.Contains(customerSession.LastIpAddress))
                {
                    customerSessions.Remove(customerSession);
                    if (customers.Contains(customerSession))
                        customers.Remove(customerSession);
                }
                alreadyUsedIPs.Add(customerSession.LastIpAddress);
            }

            //var totalSessionCount = (await _customerService.GetTotalSessionsAsync(DateTime.UtcNow, DateTime.Today)).Count;

            var salesSummary = await GetSalesSummaryReportAsync();
            if(salesSummary != null && salesSummary.Count > 0)
            {
               var profit = String.Format("{0:C}", salesSummary.Sum(x => x.Profit));

                await _hubContext.Clients.All.SendAsync("TotalDailySales", profit);
                await _hubContext.Clients.All.SendAsync("TotalDailyOrders", salesSummary.Sum(x => x.NumberOfOrders).ToString());
            }

            await _hubContext.Clients.All.SendAsync("TotalSessionCount", customerSessions.Count);

            await _hubContext.Clients.All.SendAsync("VisitorCount", customers.Count());
        }

        public async Task<IPagedList<SalesSummaryReportLine>> GetSalesSummaryReportAsync()
        {
            //get parameters to filter orders
            OrderStatus? orderStatus = null;
            PaymentStatus? paymentStatus = null;

            var currentVendor = await _workContext.GetCurrentVendorAsync();
            //var offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
            var startDateValue = DateTime.Today;

            //hack... trying to fix issues between docker datetime and windows
            var endDateValue = DateTime.UtcNow;
            if (endDateValue.AddHours(_timeZone.BaseUtcOffset.Hours) < DateTime.Today)
                startDateValue = startDateValue.AddDays(-1);


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
