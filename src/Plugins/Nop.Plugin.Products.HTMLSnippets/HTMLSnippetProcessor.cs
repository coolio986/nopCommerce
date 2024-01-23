using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Services.Plugins;
using Nop.Services.Cms;
using Nop.Web.Framework.Menu;
using Nop.Web.Framework.Infrastructure;
using Nop.Plugin.Product.HTMLSnippets;
using Nop.Services.Security;
using Nop.Core.Domain.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Product.HTMLSnippets.Components;

namespace Nop.Plugin.Product.HTMLSnippets
{
    public class HTMLSnippetProcessor : BasePlugin, IWidgetPlugin, IAdminMenuPlugin
    {
        public bool HideInWidgetList => true;

        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly WidgetSettings _widgetSettings;
        private readonly ILocalizationService _localizationService;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IActionContextAccessor _actionContextAccessor;

        public HTMLSnippetProcessor(IPermissionService permissionService, ILocalizationService localizationService, 
            ISettingService settingService, WidgetSettings widgetSettings, IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor)
        {
            _permissionService = permissionService;
            _settingService = settingService;
            _widgetSettings = widgetSettings;
            _localizationService = localizationService;
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccessor = actionContextAccessor;
        }

        public string GetWidgetViewComponentName(string widgetZone)
        {
            if (widgetZone.Equals(AdminWidgetZones.ProductDetailsBlock))
                return HTMLSnippetDefaults.PRODUCT_DETAILS_VIEW_COMPONENT_NAME;

            if (widgetZone.Equals(PublicWidgetZones.ProductDetailsOverviewBottom))
                return HTMLSnippetDefaults.SNIPPET_VIEW_COMPONENT_NAME;

            //if (widgetZone.Equals(AdminWidgetZones.OrderShipmentDetailsButtons))
            //    return EasyPostDefaults.SHIPMENT_DETAILS_VIEW_COMPONENT_NAME;

            //if (widgetZone.Equals(PublicWidgetZones.OpCheckoutShippingMethodTop))
            //    return EasyPostDefaults.SHIPPING_METHODS_VIEW_COMPONENT_NAME;

            return null;
        }

        /// <summary>
        /// Gets a type of a view component for displaying widget
        /// </summary>
        /// <param name="widgetZone">Name of the widget zone</param>
        /// <returns>View component type</returns>
        public Type GetWidgetViewComponent(string widgetZone)
        {
            return typeof(HTMLSnippetsViewComponent);
        }

        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string>
            {
                AdminWidgetZones.ProductDetailsBlock,
                PublicWidgetZones.ProductDetailsOverviewBottom,
            });
        }

        public async Task ManageSiteMapAsync(SiteMapNode rootNode)
        {
            //if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
            //    return;
        }

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext).RouteUrl(HTMLSnippetDefaults.ConfigurationRouteName);
        }

        public override async Task InstallAsync()
        {
            await _settingService.SaveSettingAsync(new HTMLSnippetsSettings
            {
                PluginEnabled = true,
                Snippets = new List<Snippet>(),
            })
            ;

            if (!_widgetSettings.ActiveWidgetSystemNames.Contains(HTMLSnippetDefaults.SystemName))
            {
                _widgetSettings.ActiveWidgetSystemNames.Add(HTMLSnippetDefaults.SystemName);
                await _settingService.SaveSettingAsync(_widgetSettings);
            }

            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.Product.HTMLSnippets.PluginEnabled"] = "Plugin Enabled",
                ["Plugins.Product.HTMLSnippets.Configuration.Header"] = "Configuration",
                ["Plugins.Product.HTMLSnippets"] = "HTMLSnippet",
                ["Plugins.Product.HTMLSnippets.SnippetName"] = "Snippet Name",
                ["Plugins.Product.HTMLSnippets.SnippetHTML"] = "HTML",
                ["Plugins.Product.HTMLSnippets.SelectedSnippet"] = "Selected Snippet",
            });

            ;

            await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {

            await _settingService.DeleteSettingAsync<HTMLSnippetsSettings>();

            if (_widgetSettings.ActiveWidgetSystemNames.Contains(HTMLSnippetDefaults.SystemName))
            {
                _widgetSettings.ActiveWidgetSystemNames.Remove(HTMLSnippetDefaults.SystemName);
                await _settingService.SaveSettingAsync(_widgetSettings);
            }

            

            await base.UninstallAsync();
        }
    }
}
