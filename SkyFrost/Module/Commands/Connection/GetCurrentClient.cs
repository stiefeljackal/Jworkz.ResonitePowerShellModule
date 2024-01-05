using System.Management.Automation;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Commands.Connection;

using Core.Commands.Abstract;

/// <summary>
/// Retrieves the current CloudX client used as the default client
/// </summary>
[Cmdlet(VerbsCommon.Get, "ResoniteCurrentSkyFrostClient")]
public class GetCurrentClient : BasePSCmdlet
{
    protected override void ExecuteCmdlet()
    {
        WriteObject(SkyFrostInterfacePool.Current);
    }
}
