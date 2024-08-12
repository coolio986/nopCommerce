using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Nop.Core.Configuration;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Product.HTMLSnippets
{
    public class HTMLSnippetsSettings : ISettings
    {
        
        public bool PluginEnabled { get; set; }

        public string SerializedSettings { get; set; }

        public List<Snippet> Snippets { get; set; }

        public HTMLSnippetsSettings()
        {
            Snippets = new List<Snippet>();
        }

    }
    public class Snippet : ISettings
    {
        [NopResourceDisplayName("Plugins.Product.HTMLSnippets.SnippetName")]
        public string Name { get; set; }

        [NopResourceDisplayName("Plugins.Product.HTMLSnippets.SnippetHTML")]
        public string HTML { get; set; }

        [JsonIgnore]
        public int Index { get; set; }

    }

}
