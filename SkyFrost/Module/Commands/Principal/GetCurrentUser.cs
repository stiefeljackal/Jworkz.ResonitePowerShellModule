using System.Management.Automation;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Commands.Principal;

using Commands.Abstract;

/// <summary>
/// Retrieves the current user who had connected to the SkyFrost infrastructure using this interface.
/// </summary>
[Cmdlet(VerbsCommon.Get, "SfCurrentUser")]
public class GetCurrentUser : SkyFrostConnectedCmdlet
{
    protected override void ExecuteCmdlet()
    {
        WriteObject(Client!.CurrentUser);
    }
}