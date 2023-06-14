using JworkzNeosPowerShellModule.Commands.Abstract;
using JworkzNeosPowerShellModule.Secrets.Abstract;
using JworkzNeosPowerShellModule.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace JworkzNeosPowerShellModule.Commands.Auth
{
    /// <summary>
    /// Retrieves a stored NeosVR Credential from a secrets management engine
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "NeosStoredCredential")]
    public class GetStoredCredential : BasePSCmdlet
    {
        /// <summary>
        /// Key used to retrieve the credentials
        /// </summary>
        [Parameter(Mandatory = true)]
        public string Key = "";

        /// <summary>
        /// Optional secrets management engine to use over the current default engine
        /// </summary>
        [Parameter]
        public ISecretsManagement? SecretsManagement;

        protected override void ProcessRecord()
        {
            var secretsManagement = SecretsManagement ?? SecretsManager.Current;
            var creds = secretsManagement?.GetCredential(Key);

            if (creds != null)
            {
                WriteObject(creds);
            }
            else
            {
                WriteError(new ErrorRecord(new Exception("Credentials not found"), "CREDSNOTFOUND", ErrorCategory.AuthenticationError, this));
            }
        }
    }
}
