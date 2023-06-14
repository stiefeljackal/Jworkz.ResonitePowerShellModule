using JworkzNeosPowerShellModule.Clients.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace JworkzNeosPowerShellModule.Commands.Connection;

using Abstract;
using Utilities;

/// <summary>
/// Disconnects from the NeosVR Api interface via CloudX with either the default or provided client
/// </summary>
[Cmdlet("Disconnect", "NeosApi")]
[OutputType(typeof(void))]
public class DisconnectApi : BasePSCmdlet
{
    /// <summary>
    /// Optional client to send the request to intead of the current default one
    /// </summary>
    [Parameter]
    public ICloudInterfaceClient? Client;

    protected override void ProcessRecord()
    {
        if (!IsParamSpecified("Client"))
        {
            Client = CloudXInterfacePool.Current ?? throw new InvalidOperationException(ErrorActionSpecified);
        }
        else if (Client == null)
        {
            throw new InvalidOperationException("Specified client is null.");
        }

        Client?.Logout();

        if (Client == CloudXInterfacePool.Current)
        {
            CloudXInterfacePool.Current = null;
        }
    }
}
