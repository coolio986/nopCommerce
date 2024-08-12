using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Product.HTMLSnippets
{
    public class HTMLSnippetDefaults
    {
        /// <summary>
        /// Gets the plugin system name
        /// </summary>
        public static string SystemName => "Product.HTMLSnippets";

        /// <summary>
        /// Gets the configuration route name
        /// </summary>
        public static string ConfigurationRouteName => "Plugin.Product.HTMLSnippets.Configure";

        public static string AddSnippetRouteName => "Plugin.Product.HTMLSnippets.AddSnippet";

        public static string ProductHTMLSnippetAttributeFormKey => "SelectedSnippet";

        public static string ProductHTMLSnippetAttribute => "HTMLSnippet.Product.SelectedSnippet";

        #region View components

        /// <summary>
        /// Gets the name of the view component to display an additional block on the product details page in the admin area
        /// </summary>
        public const string PRODUCT_DETAILS_VIEW_COMPONENT_NAME = "HTMLSnippets.ProductDetailsBlock";

        public const string SNIPPET_VIEW_COMPONENT_NAME = "HTMLSnippets";

        ///// <summary>
        ///// Gets the name of the view component to display an additional block on the shipment details page in the admin area
        ///// </summary>
        //public const string SHIPMENT_DETAILS_VIEW_COMPONENT_NAME = "HTMLSnippet.ShipmentDetailsBlock";

        ///// <summary>
        ///// Gets the name of the view component to display address verification warning on the opc shipping methods page in the public store
        ///// </summary>
        //public const string SHIPPING_METHODS_VIEW_COMPONENT_NAME = "HTMLSnippet.OpcShippingMethods";

        #endregion
    }
}
