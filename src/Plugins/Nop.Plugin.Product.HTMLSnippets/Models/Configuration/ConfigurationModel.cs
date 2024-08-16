using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Product.HTMLSnippets.Models.Configuration
{
    /// <summary>
    /// Represents configuration model
    /// </summary>
    public record ConfigurationModel : BaseNopModel
    {
        #region Ctor

        public ConfigurationModel()
        {
            Snippets = new List<Snippet>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Plugins.Product.HTMLSnippets.PluginEnabled")]
        public bool PluginEnabled { get; set; }

        public string SerializedSettings { get; set; }

        public List<Snippet> Snippets { get; set; }

        #endregion
    }

    
}