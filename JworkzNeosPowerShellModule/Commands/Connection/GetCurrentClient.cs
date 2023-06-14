using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JworkzNeosPowerShellModule.Commands.Connection;

using Abstract;
using JworkzNeosPowerShellModule.Utilities;
using System.Management.Automation;

/// <summary>
/// Retrieves the current CloudX client used as the default client
/// </summary>
[Cmdlet(VerbsCommon.Get, "NeosCurrentCloudXClient")]
public class GetCurrentClient : BasePSCmdlet
{
    protected override void ProcessRecord()
    {
        WriteObject(CloudXInterfacePool.Current);
    }
}
