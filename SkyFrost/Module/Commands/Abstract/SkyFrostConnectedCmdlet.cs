using System.Management.Automation;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Commands.Abstract;

using Clients.Abstract;
using Core.Commands.Abstract;
using Core.Models.Abstract;

/// <summary>
/// Base class for cmdlets that require a SkyFrost connection.
/// </summary>
public class SkyFrostConnectedCmdlet : BasePSCmdlet
{
    /// <summary>
    /// Client to use for SkyFrost Api calls
    /// </summary>
    [Parameter(HelpMessage = "Optional client to be used. Defaults to the default current client if null.")]
    public ISkyFrostInterfaceClient? Client;

    public SkyFrostConnectedCmdlet() : base() { }

    public SkyFrostConnectedCmdlet(IFileSystem fileSystem) : base(fileSystem) { }

    protected override void PerformPreprocessSetup()
    {
        if (Client == null)
        {
            Client = SkyFrostInterfacePool.Current
                ?? throw new InvalidOperationException("A client has not been established yet. Use Connect-SfConnectApi to connect or provide a valid client using -Client.");
        }
    }
}
