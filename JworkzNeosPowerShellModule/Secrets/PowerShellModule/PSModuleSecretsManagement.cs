using JworkzNeosPowerShellModule.Secrets.Abstract;
using JworkzNeosPowerShellModule.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace JworkzNeosPowerShellModule.Secrets.PowerShellModule
{
    public class PSModuleSecretsManagement : ISecretsManagement
    {
        private string _defaultValueName = string.Empty;

        public string DefaultVaultName
        {
            get
            {
                if (string.IsNullOrEmpty(_defaultValueName))
                {
                    _defaultValueName = GetDefaultVaultName();
                }
                return _defaultValueName;
            }
            set => _defaultValueName = value;
        }

        public bool IsDefaultVaultNameAvailable
        {
            get => !string.IsNullOrEmpty(DefaultVaultName);
        }

        public bool AddCredential(string key, string username, SecureString password, bool overwrite)
        {
            if (!IsDefaultVaultNameAvailable) { return false; }

            var creds = new PSCredential(username, password);
            GeneralUtility.StartPowerShellSession((ps) =>
            {
                ps.AddCommand("Set-Secret")
                    .AddParameter("Vault", DefaultVaultName)
                    .AddParameter("Name", key)
                    .AddParameter("Secret", creds);

                ps.Invoke();
            });

            return true;
        }

        public PSCredential? GetCredential(string key)
        {
            if (!IsDefaultVaultNameAvailable) { return null; }

            PSCredential? creds = null;

            GeneralUtility.StartPowerShellSession((ps) =>
            {
                ps.AddCommand("Get-Secret")
                    .AddParameter("Vault", DefaultVaultName)
                    .AddParameter("Name", key);

                var result = ps.Invoke().FirstOrDefault();

                var username = result?.Properties["Username"].Value.ToString();
                var password = result?.Properties["Password"].Value as SecureString;
                creds = new PSCredential(username, password);
            });

            return creds;
        }

        public bool RemoveCredential(string key)
        {
            if (!IsDefaultVaultNameAvailable) { return false; }

            GeneralUtility.StartPowerShellSession((ps) =>
            {
                ps.AddCommand("Remove-Secret")
                    .AddParameter("Vault", DefaultVaultName)
                    .AddParameter("Name", key);

                ps.Invoke();
            });

            return true;
        }

        private static string GetDefaultVaultName()
        {
            var defaultVaultName = string.Empty;

            GeneralUtility.StartPowerShellSession((ps) =>
            {
                ps.AddCommand("Get-SecretVault");

                foreach (var result in ps.Invoke())
                {
                    var isDefaultProp = result.Properties.FirstOrDefault(p => p.Name == "IaDefault");

                    if (isDefaultProp != null && Convert.ToBoolean(isDefaultProp.Value))
                    {
                        try
                        {
                            defaultVaultName = result.Properties["Name"].Value.ToString();
                        }
                        catch
                        {
                            defaultVaultName = result.Properties["VaultName"].Value.ToString();
                        }
                    }
                }
            });


            return defaultVaultName;
        }
    }
}
