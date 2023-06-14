using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace JworkzNeosPowerShellModule.Secrets.Abstract
{
    public interface ISecretsManagement
    {
        bool AddCredential(string key, string username, SecureString password, bool overwrite);
        PSCredential? GetCredential(string key);
        bool RemoveCredential(string key);
    }
}
