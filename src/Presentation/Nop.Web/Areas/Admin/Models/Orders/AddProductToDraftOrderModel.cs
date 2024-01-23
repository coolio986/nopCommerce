using System.Collections.Generic;
using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Orders
{
    /// <summary>
    /// Represents a product model to add to the category
    /// </summary>
    public partial record AddProductToDraftOrderModel : BaseNopModel
    {
        #region Ctor

        public AddProductToDraftOrderModel()
        {
            SelectedProductIds = new List<int>();
        }
        #endregion

        #region Properties

        public int DraftOrderId { get; set; }

        public IList<int> SelectedProductIds { get; set; }

        #endregion
    }
}