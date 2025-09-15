using System.Management.Automation;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Commands.Connection;

using Core.Commands.Abstract;

/// <summary>
/// Retrieves the current SkyFrost client used as the default client.
/// </summary>
[Cmdlet(VerbsCommon.Get, "SfCurrentClient")]
public class GetCurrentClient : BasePSCmdlet
{
    protected override void ExecuteCmdlet()
    {
        WriteObject(SkyFrostInterfacePool.Current);
    }
}
