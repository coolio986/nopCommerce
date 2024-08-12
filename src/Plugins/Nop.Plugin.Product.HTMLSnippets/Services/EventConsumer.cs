using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Events;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Product.HTMLSnippets.Services
{
    public class EventConsumer : IConsumer<ModelReceivedEvent<BaseNopModel>>
    {
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IProductService _productService;
        private readonly IPermissionService _permissionService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EventConsumer(IGenericAttributeService genericAttributeService, 
            IProductService productService, 
            IPermissionService permissionService,
            IHttpContextAccessor httpContextAccessor)
        {
            _genericAttributeService = genericAttributeService;
            _productService = productService;
            _permissionService = permissionService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task HandleEventAsync(ModelReceivedEvent<BaseNopModel> eventMessage)
        {
            if (eventMessage.Model is not ProductModel model)
                return;

            

            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageShippingSettings))
                return;

            var product = await _productService.GetProductByIdAsync(model.Id);
            if (product is null)
                return;

            //try to get additional form values for the product
            var form = _httpContextAccessor.HttpContext.Request.Form;
            if (form.TryGetValue(HTMLSnippetDefaults.ProductHTMLSnippetAttributeFormKey, out var selectedSnippetValue))
            {
                var selectedSnippet = !StringValues.IsNullOrEmpty(selectedSnippetValue) ? selectedSnippetValue.ToString() : null;
                await _genericAttributeService.SaveAttributeAsync(product, HTMLSnippetDefaults.ProductHTMLSnippetAttribute, selectedSnippet);
            }

        }
    }
}
