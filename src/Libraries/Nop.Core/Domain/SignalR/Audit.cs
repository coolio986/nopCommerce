using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.SignalR
{
    public class Audit
    {
        public Guid AuditId { get; set; }
        public int Id { get; set; }
        public DateTime LastInserted { get; set; }
        public string TriggerType { get; set; }

    }
}
