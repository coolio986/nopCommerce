using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Configuration
{
    public partial class SignalRConfig : IConfig
    {
        public string OrderEventURL { get; private set; } = "localhost";
    }
}
