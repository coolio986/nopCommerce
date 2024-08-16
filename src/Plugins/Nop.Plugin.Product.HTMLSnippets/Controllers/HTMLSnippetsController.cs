using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Nop.Services.Security;
using Nop.Web.Framework.Controllers;
using Nop.Plugin.Product.HTMLSnippets.Models.Configuration;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework;
using Nop.Services.Configuration;
using Nop.Services.Messages;
using Nop.Services.Localization;
using System.ServiceModel.Channels;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using Nop.Core.Domain.Catalog;
using Nop.Services.Plugins;
using DocumentFormat.OpenXml.EMMA;
using Newtonsoft.Json;

namespace Nop.Plugin.Product.HTMLSnippets.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.ADMIN)]
    [AutoValidateAntiforgeryToken]
    public class HTMLSnippetsController : BasePluginController
    {

        private readonly IPermissionService _permissionService;
        private readonly HTMLSnippetsSettings _hTMLSnippetsSettings;
        private readonly ISettingService _settingService;
        private readonly INotificationService _notificationService;
        private readonly ILocalizationService _localizationService;

        public HTMLSnippetsController(IPermissionService permissionService, HTMLSnippetsSettings hTMLSnippetsSettings, ISettingService settingService, 
            INotificationService notificationService, ILocalizationService localizationService)
        {
            _permissionService = permissionService;
            _hTMLSnippetsSettings = hTMLSnippetsSettings;
            _settingService = settingService;
            _notificationService = notificationService;
            _localizationService = localizationService;
        }
        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageShippingSettings))
                return AccessDeniedView();

            var model = new ConfigurationModel
            {
                PluginEnabled = _hTMLSnippetsSettings.PluginEnabled,
                SerializedSettings = _hTMLSnippetsSettings.SerializedSettings,
                Snippets = new List<Snippet>()
            };

            if (model.SerializedSettings != null && model.SerializedSettings != string.Empty)
                model.Snippets = JsonConvert.DeserializeObject<List<Snippet>>(model.SerializedSettings);
            
            return View("~/Plugins/Product.HTMLSnippets/Views/Configuraton/Configure.cshtml", model);
            
            
        }
        
        [HttpPost]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageShippingSettings))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return await Configure();

            _hTMLSnippetsSettings.PluginEnabled = model.PluginEnabled;
            _hTMLSnippetsSettings.SerializedSettings = JsonConvert.SerializeObject(model.Snippets);
            _hTMLSnippetsSettings.Snippets = model.Snippets;
            await _settingService.SaveSettingAsync(_hTMLSnippetsSettings);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return await Configure();
        }

        [HttpGet]
        public IActionResult AddSnippet(int index)
        {
            var snippet = new Snippet() { Name = string.Empty, HTML=string.Empty, Index = index  };
            
            return PartialView("~/Plugins/Product.HTMLSnippets/Views/Configuraton/AddSnippet.cshtml", snippet);
        }
    }
}
