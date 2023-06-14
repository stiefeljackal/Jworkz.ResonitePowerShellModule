using JworkzNeosPowerShellModule.Commands.Abstract;
using JworkzNeosPowerShellModule.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace JworkzNeosPowerShellModule.Commands.Connection;

/// <summary>
/// Pings the NeosVR Api interface
/// </summary>
[Cmdlet(VerbsCommon.Get, "NeosPing")]
public class PingApi : NeosConnectedCmdlet
{
    protected override void ProcessRecord()
    {
        var isPingable = Client!.IsPingable().GetAwaiterResult();
        WriteObject(isPingable);
    }
}
