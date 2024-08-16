using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Presentation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Product.HTMLSnippets.Models.Products
{
    public record ProductDetailsModel : BaseNopModel
    {

        public ProductDetailsModel()
        {
            AvailableSnippets = new List<SelectListItem>();
        }
        public IList<SelectListItem> AvailableSnippets { get; set; }

        [NopResourceDisplayName("Plugins.Product.HTMLSnippets.SelectedSnippet")]
        public string SelectedSnippet { get; set; }

    }
}
