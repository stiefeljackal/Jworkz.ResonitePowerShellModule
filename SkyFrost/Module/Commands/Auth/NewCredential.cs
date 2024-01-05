using System.Management.Automation;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Commands.Auth;

using Core.Commands.Abstract;
using Core.Utilities;

/// <summary>
/// Creates a Resonite Credential as a PSCredential object
/// </summary>
[Cmdlet(VerbsCommon.New, "ResoniteCredential")]
public class NewCredential : BasePSCmdlet
{
    /// <summary>
    /// Username of the account
    /// </summary>
    [Parameter(Mandatory = true)]
    public string Username = string.Empty;

    protected override void PrepareCmdlet()
    {
        Username.ThrowOnNullOrEmpty("Username cannot be null or empty.");
    }

    protected override void ExecuteCmdlet()
    {

        var password = Host.PromptForPassword();

        if (password == null || password.Length <= 0)
        {
            this.WritePasswordEmptyOrNullError();
        }
        else
        {
            WriteObject(new PSCredential(Username!, password));
        }
    }
}
