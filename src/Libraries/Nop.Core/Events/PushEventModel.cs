using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Orders;

namespace Nop.Core.Events
{
    public class PushEventModel
    {
        public OrderItem OrderItem { get; set; }
    }
}
