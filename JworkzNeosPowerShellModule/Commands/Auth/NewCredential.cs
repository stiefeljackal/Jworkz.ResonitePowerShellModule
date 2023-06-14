using JworkzNeosPowerShellModule.Commands.Abstract;
using JworkzNeosPowerShellModule.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace JworkzNeosPowerShellModule.Commands.Auth
{
    /// <summary>
    /// Creates a NeosVR Credential as a PSCredential object
    /// </summary>
    [Cmdlet(VerbsCommon.New, "NeosCredential")]
    public class NewCredential : BasePSCmdlet
    {
        /// <summary>
        /// Username of the NeosVR account
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
}
