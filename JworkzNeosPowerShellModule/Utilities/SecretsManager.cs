using JworkzNeosPowerShellModule.Secrets.Abstract;
using JworkzNeosPowerShellModule.Secrets.PowerShellModule;
using JworkzNeosPowerShellModule.Secrets.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace JworkzNeosPowerShellModule.Utilities;

internal static class SecretsManager
{
    private static ISecretsManagement? _current;

    /// <summary>
    /// Current secrets management being used for this session
    /// </summary>
    public static ISecretsManagement? Current
    {
        get
        {
            if (_current == null)
            {
                _current = GetDefault();
            }

            return _current;
        }
        set => _current = value;
    }

    /// <summary>
    /// Retrieves the class type of the current secrets managaement
    /// </summary>
    public static Type? CurrentType { get => Current?.GetType(); }

    /// <summary>
    /// Retrieves the default secrets management based on available modules and OS
    /// </summary>
    /// <returns>Default secrets management instance or null</returns>
    private static ISecretsManagement? GetDefault()
    {
        ISecretsManagement? secretsManagement = null;

        if (GeneralUtility.HasPSSecretManagementModule())
        {
            secretsManagement = new PSModuleSecretsManagement();
        }
        else if (OperatingSystem.IsWindows())
        {
            secretsManagement = new WindowsVaultSecretsManagement();
        }

        return secretsManagement;
    }
}
