using System.Management.Automation;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Commands.Connection;

using Core.Commands.Abstract;
using Clients.Abstract;

/// <summary>
/// Disconnects from the SkyFrost Api interface with either the default or provided client
/// </summary>
[Cmdlet("Disconnect", "SfApi")]
[OutputType(typeof(void))]
public class DisconnectApi : BasePSCmdlet
{
    /// <summary>
    /// Optional client to send the request to intead of the current default one
    /// </summary>
    [Parameter]
    public ISkyFrostInterfaceClient? Client;

    protected override void PrepareCmdlet()
    {
        if (!IsParamSpecified("Client"))
        {
            Client = SkyFrostInterfacePool.Current ?? throw new InvalidOperationException(ErrorActionSpecified);
        }
        else if (Client == null)
        {
            throw new InvalidOperationException("Specified client is null.");
        }
    }

    protected override void ExecuteCmdlet()
    {
        Client!.Logout();

        if (Client == SkyFrostInterfacePool.Current)
        {
            SkyFrostInterfacePool.Current = null;
        }
    }
}
