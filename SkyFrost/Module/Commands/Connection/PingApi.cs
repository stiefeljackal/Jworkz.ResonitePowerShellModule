using System.Management.Automation;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Commands.Connection;

using Commands.Abstract;
using Core.Utilities;

/// <summary>
/// Pings the SkyFrost Api interface.
/// </summary>
[Cmdlet(VerbsCommon.Get, "SfPing")]
public class PingApi : SkyFrostConnectedCmdlet
{
    protected override void ExecuteCmdlet()
    {
        var isPingable = Client!.IsPingable().GetAwaiterResult();
        WriteObject(isPingable);
    }
}
