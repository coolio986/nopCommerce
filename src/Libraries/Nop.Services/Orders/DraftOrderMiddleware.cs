using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Nop.Core;
using Nop.Core.Infrastructure;
using Nop.Core.Domain.Orders;


namespace Nop.Services.Orders
{
    public class DraftOrderMiddleware
    {
        private readonly RequestDelegate _next;

        //private readonly IShoppingCartService _shoppingCartService;
        //private readonly IStoreContext _storeContext;
        //private readonly IWorkContext _workContext;
        //private readonly IDraftOrderService _draftOrderService;


        public IAuthenticationSchemeProvider Schemes { get; set; }

        public DraftOrderMiddleware(IAuthenticationSchemeProvider schemes, RequestDelegate next)
        {
            Schemes = schemes ?? throw new ArgumentNullException(nameof(schemes));
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }
        /// <summary>
        /// Invoke middleware actions
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.HasValue && !context.Request.Path.Value.ToLower().StartsWith("/checkout"))
            {
                var draftOrderService = EngineContext.Current.Resolve<IDraftOrderService>();
                var workContext = EngineContext.Current.Resolve<IWorkContext>();
                var draftOrderGuidCookie = workContext.GetDraftOrderCookie();
                if (draftOrderGuidCookie != null )
                {

                    var shoppingCartService = EngineContext.Current.Resolve<IShoppingCartService>();
                    var storeContext = EngineContext.Current.Resolve<IStoreContext>();
                    var store = await storeContext.GetCurrentStoreAsync();
                    var cart = await shoppingCartService.GetShoppingCartAsync(await workContext.GetCurrentCustomerAsync(), ShoppingCartType.ShoppingCart, store.Id);

                    foreach (var item in cart)
                    {
                        await shoppingCartService.DeleteShoppingCartItemAsync(item);
                    }
                    workContext.DeleteDraftOrderCookie();
                }
            }
            await _next(context);
        }
    }
}
