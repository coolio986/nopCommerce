using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Product.HTMLSnippets.Infrastructure
{
    /// <summary>
    /// Represents plugin route provider
    /// </summary>
    public class RouteProvider : IRouteProvider
    {
        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="endpointRouteBuilder">Route builder</param>
        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapControllerRoute(name: HTMLSnippetDefaults.ConfigurationRouteName,
                pattern: "Admin/HTMLSnippets/Configure",
                defaults: new { controller = "HTMLSnippets", action = "Configure" });

            endpointRouteBuilder.MapControllerRoute(name: HTMLSnippetDefaults.AddSnippetRouteName,
                pattern: "Admin/HTMLSnippets/AddSnippet",
                defaults: new { controller = "HTMLSnippets", action = "AddSnippet" });
        }

        /// <summary>
        /// Gets a priority of route provider
        /// </summary>
        public int Priority => 0;
    }
}