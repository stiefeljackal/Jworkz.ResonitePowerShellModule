using System.Management.Automation;

namespace Jworkz.ResonitePowerShellModule.Core.Commands.Principal;

using Commands.Abstract;

/// <summary>
/// Retrieves the current Resonite user who had connected to the cloud using this interface
/// </summary>
[Cmdlet(VerbsCommon.Get, "ResoniteCurrentUser")]
public class GetCurrentUser : ResoniteConnectedCmdlet
{
    protected override void ExecuteCmdlet()
    {
        WriteObject(Client!.CurrentUser);
    }
}