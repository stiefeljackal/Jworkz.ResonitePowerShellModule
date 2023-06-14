using JworkzNeosPowerShellModule.Commands.Abstract;
using JworkzNeosPowerShellModule.Secrets.Abstract;
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
    /// Adds a stored NeosVR Credential TO a secrets management engine
    /// </summary>
    [Cmdlet(VerbsCommon.Add, "NeosStoredCredential")]
    public class AddStoredCredential : BasePSCmdlet
    {
        /// <summary>
        /// Key to associate the credentials to
        /// </summary>
        [Parameter(Mandatory = true)]
        public string Key = "";

        /// <summary>
        /// Username of the account
        /// </summary>
        [Parameter(Mandatory = true)]
        public string Username = "";

        /// <summary>
        /// Password of the account
        /// </summary>
        [Parameter]
        public SecureString? Password;

        /// <summary>
        /// Whether or not to overwrite existing credentials
        /// </summary>
        [Parameter]
        public SwitchParameter Overwrite;

        /// <summary>
        /// Optional secrets management engine to use over the current default engine
        /// </summary>
        [Parameter]
        public ISecretsManagement? SecretsManagement;

        protected override void ProcessRecord()
        {
            var secretsManagement = SecretsManagement ?? SecretsManager.Current;
            
            if (Password == null || Password!.Length <= 0)
            {
                Password = Host.PromptForPassword();
            }

            if (Password!.Length > 0)
            {
                secretsManagement?.AddCredential(Key, Username, Password, Overwrite.ToBool());
            }
            else
            {
                this.WritePasswordEmptyOrNullError();
            }
        }
    }
}
