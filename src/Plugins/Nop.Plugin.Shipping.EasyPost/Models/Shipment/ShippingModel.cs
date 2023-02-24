using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyPost;

namespace Nop.Plugin.Shipping.EasyPost.Models.Shipment
{
    public class ShippingModel
    {
        public IList<string> Errors { get; set; } = new List<string>();

        public IList<ShippingOption> ShippingOptions { get; set; } = new List<ShippingOption>();

        public string Error => Errors.FirstOrDefault() == null ? "" : Errors.FirstOrDefault();

        public Parcel Parcel { get; set; } = new Parcel();
    }
}
