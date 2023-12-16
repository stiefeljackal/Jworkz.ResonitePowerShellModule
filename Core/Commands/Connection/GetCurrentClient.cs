using System.Management.Automation;

namespace Jworkz.ResonitePowerShellModule.Core.Commands.Connection;

using Abstract;
using Utilities;

/// <summary>
/// Retrieves the current CloudX client used as the default client
/// </summary>
[Cmdlet(VerbsCommon.Get, "ResoniteCurrentSkyFrostClient")]
public class GetCurrentClient : BasePSCmdlet
{
    protected override void ProcessRecord()
    {
        WriteObject(SkyFrostInterfacePool.Current);
    }
}
