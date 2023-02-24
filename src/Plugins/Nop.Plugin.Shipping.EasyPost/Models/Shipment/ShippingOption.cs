using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Shipping.EasyPost.Models.Shipment
{
    /// <summary>
    /// Represents a shipping option
    /// </summary>
    public partial class ShippingOption
    {
        /// <summary>
        /// Gets or sets a shipping rate (without discounts, additional shipping charges, etc)
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// Gets or sets a shipping option name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a shipping option description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a transit days
        /// </summary>
        public int? TransitDays { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if it's pickup in store shipping option
        /// </summary>
        public bool IsPickupInStore { get; set; }

        /// <summary>
        /// Gets or sets a display order
        /// </summary>
        public int? DisplayOrder { get; set; }

        public bool Selected { get; set; }


    }
}
