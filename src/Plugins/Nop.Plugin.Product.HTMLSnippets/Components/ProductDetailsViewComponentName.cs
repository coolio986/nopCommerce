using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Product.HTMLSnippets.Models.Configuration;
using Nop.Plugin.Product.HTMLSnippets.Models.Products;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Directory;
using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Services.Shipping;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Components;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Product.HTMLSnippets.Components
{
    /// <summary>
    /// Represents view component to render an additional block on the product details page in the admin area
    /// </summary>
    [ViewComponent(Name = HTMLSnippetDefaults.PRODUCT_DETAILS_VIEW_COMPONENT_NAME)]
    public class ProductDetailsViewComponentName : NopViewComponent
    {
        #region Fields

        private readonly ICountryService _countryService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IPermissionService _permissionService;
        private readonly IProductService _productService;
        private readonly HTMLSnippetsSettings _hTMLSnippetsSettings;

        #endregion

        #region Ctor

        public ProductDetailsViewComponentName(ICountryService countryService,
            IGenericAttributeService genericAttributeService,
            IPermissionService permissionService,
            IProductService productService,
            HTMLSnippetsSettings hTMLSnippetsSettings)
        {
            _countryService = countryService;
            _genericAttributeService = genericAttributeService;
            _permissionService = permissionService;
            _productService = productService;
            _hTMLSnippetsSettings = hTMLSnippetsSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invoke the widget view component
        /// </summary>
        /// <param name="widgetZone">Widget zone</param>
        /// <param name="additionalData">Additional parameters</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the view component result
        /// </returns>
        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            

            if (!widgetZone.Equals(AdminWidgetZones.ProductDetailsBlock))
                return Content(string.Empty);

            if (additionalData is not ProductModel productModel)
                return Content(string.Empty);

            var product = await _productService.GetProductByIdAsync(productModel.Id);
            if (product is null)
                return Content(string.Empty);

            var selectedSnippet = await _genericAttributeService
                .GetAttributeAsync<string>(product, HTMLSnippetDefaults.ProductHTMLSnippetAttribute) ?? string.Empty;

            List<Snippet> configModel = JsonConvert.DeserializeObject<List<Snippet>>(_hTMLSnippetsSettings.SerializedSettings);

            var model = new ProductDetailsModel
            {
                AvailableSnippets = configModel.Select(x => new SelectListItem( x.Name, x.Name)).ToList(),
                SelectedSnippet = selectedSnippet
            };
            model.AvailableSnippets.Insert(0, new SelectListItem("---", string.Empty));


            return View("~/Plugins/Product.HTMLSnippets/Views/Product/_CreateOrUpdate.HTMLSnippet.cshtml", model);
            
            
        }

        #endregion
    }
}