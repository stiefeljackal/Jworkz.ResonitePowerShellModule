using JworkzNeosPowerShellModule.Clients.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JworkzNeosPowerShellModule.Utilities
{
    public static class CloudXInterfacePool
    {
        /// <summary>
        /// Current default client that is used for all cmdlets unless a specific client is provided
        /// </summary>
        public static ICloudInterfaceClient? Current;
    }
}
