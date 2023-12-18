using System.Management.Automation;

namespace Jworkz.ResonitePowerShellModule.Core.Commands.Connection;

using Commands.Abstract;
using Utilities;

/// <summary>
/// Pings the Resonite Api interface
/// </summary>
[Cmdlet(VerbsCommon.Get, "ResonitePing")]
public class PingApi : ResoniteConnectedCmdlet
{
    protected override void ExecuteCmdlet()
    {
        var isPingable = Client!.IsPingable().GetAwaiterResult();
        WriteObject(isPingable);
    }
}
