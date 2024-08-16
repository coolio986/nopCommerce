using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Nop.Core;
using Nop.Services.Catalog;
using Nop.Web.Framework.Components;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Models.Catalog;
using Nop.Services.Common;
using Nop.Core.Domain.Catalog;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Services.Configuration;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.Product.HTMLSnippets.Components
{
    [ViewComponent(Name = HTMLSnippetDefaults.SNIPPET_VIEW_COMPONENT_NAME)]
    public class HTMLSnippetsViewComponent : NopViewComponent
    {
        private readonly IProductService _productService;
        private readonly IStoreContext _storeContext;
        private readonly IWorkContext _workContext;
        private readonly HTMLSnippetsSettings _hTMLSnippetsSettings;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ISettingService _settingService;

        public HTMLSnippetsViewComponent(
            IProductService productService,
            IStoreContext storeContext,
            IWorkContext workContext,
            HTMLSnippetsSettings hTMLSnippetsSettings,
            IGenericAttributeService genericAttributeService)
        {
            _productService = productService;
            _storeContext = storeContext;
            _workContext = workContext;
            _hTMLSnippetsSettings = hTMLSnippetsSettings;
            _genericAttributeService = genericAttributeService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            var customer = await _workContext.GetCurrentCustomerAsync();
            var store = await _storeContext.GetCurrentStoreAsync();

            if (!_hTMLSnippetsSettings.PluginEnabled)
                return Content(string.Empty);

            if (!widgetZone.Equals(PublicWidgetZones.ProductDetailsOverviewBottom))
                return Content(string.Empty);

            var productId = additionalData is ProductDetailsModel model ? model.Id : 0;

            var product = await _productService.GetProductByIdAsync(productId);
            if (product is null)
                return Content(string.Empty);

            var selectedSnippet = await _genericAttributeService
                .GetAttributeAsync<string>(product, HTMLSnippetDefaults.ProductHTMLSnippetAttribute) ?? string.Empty;

            List<Snippet> snippetList = new List<Snippet>();

            if(_hTMLSnippetsSettings.SerializedSettings != null && _hTMLSnippetsSettings.SerializedSettings != string.Empty)
                snippetList = JsonConvert.DeserializeObject<List<Snippet>>(_hTMLSnippetsSettings.SerializedSettings);

            var snippetHTML = snippetList.FirstOrDefault(x => x.Name == selectedSnippet)?.HTML;

            if (snippetHTML != null)
                return View("~/Plugins/Product.HTMLSnippets/Views/Snippet.cshtml", (widgetZone, snippetHTML, productId));
            else
                return Content(string.Empty);

            //return View("~/Plugins/Payments.PayPalCommerce/Views/Buttons.cshtml", (widgetZone, productId, productCost));
        }

    }
}
