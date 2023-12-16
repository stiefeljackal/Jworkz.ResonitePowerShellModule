using System.Management.Automation;

namespace Jworkz.ResonitePowerShellModule.Core.Commands.Auth;

using Commands.Abstract;
using Utilities;

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
    public string Username = "";

    protected override void ProcessRecord()
    {
        if (string.IsNullOrEmpty(Username))
        {
            throw new PSArgumentException("Username cannot be null or empty.", nameof(Username));
        }

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
