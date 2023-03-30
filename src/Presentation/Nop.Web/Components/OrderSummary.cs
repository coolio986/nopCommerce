using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;
using Nop.Web.Models.ShoppingCart;

namespace Nop.Web.Components
{
    public class OrderSummaryViewComponent : NopViewComponent
    {
        private readonly IShoppingCartModelFactory _shoppingCartModelFactory;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IStoreContext _storeContext;
        private readonly IWorkContext _workContext;
        private readonly IDraftOrderService _draftOrderService;
        private readonly ILocalizationService _localizationService;

        public OrderSummaryViewComponent(IShoppingCartModelFactory shoppingCartModelFactory,
            IShoppingCartService shoppingCartService,
            IStoreContext storeContext,
            IWorkContext workContext,
            IDraftOrderService draftOrderService,
            ILocalizationService localizationService)
        {
            _shoppingCartModelFactory = shoppingCartModelFactory;
            _shoppingCartService = shoppingCartService;
            _storeContext = storeContext;
            _workContext = workContext;
            _draftOrderService = draftOrderService;
            _localizationService = localizationService;
        }

        public async Task<IViewComponentResult> InvokeAsync(bool? prepareAndDisplayOrderReviewData, ShoppingCartModel overriddenModel)
        {
            if (overriddenModel != null)
                //use already prepared (shared) model
                return View(overriddenModel);

            //if not passed, then create a new model
            var store = await _storeContext.GetCurrentStoreAsync();
            var cart = await _shoppingCartService.GetShoppingCartAsync(await _workContext.GetCurrentCustomerAsync(), ShoppingCartType.ShoppingCart, store.Id);

            var model = new ShoppingCartModel();
            model = await _shoppingCartModelFactory.PrepareShoppingCartModelAsync(model, cart,
                isEditable: false,
                prepareAndDisplayOrderReviewData: prepareAndDisplayOrderReviewData.GetValueOrDefault());

            string orderQuery = HttpContext.Request.Query["order"];
            orderQuery = orderQuery ?? Guid.Empty.ToString();

            var deletedLanguage = await _localizationService.GetResourceAsync("ShoppingCart.ProductDeleted");

            var draftOrderGuid = Guid.Parse(orderQuery);
            if (draftOrderGuid != Guid.Empty)
            {
                var draftOrder = await _draftOrderService.GetOrderByGuidAsync(draftOrderGuid);
                if (draftOrder != null)
                {
                    foreach (var item in model.Items)
                    {
                        if (item.Warnings.Contains(deletedLanguage))
                            item.Warnings.Remove(deletedLanguage);
                    }
                }
            }

            return View(model);
        }
    }
}
