using JworkzNeosPowerShellModule.Clients.Abstract;
using JworkzNeosPowerShellModule.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace JworkzNeosPowerShellModule.Commands.Abstract;

/// <summary>
/// Base class for cmdlets that require a NeosVR connection
/// </summary>
public class NeosConnectedCmdlet : BasePSCmdlet
{
    /// <summary>
    /// Client to use for NeosVR Api calls
    /// </summary>
    [Parameter(HelpMessage = "Optional client to be used. Defaults to the default current client if null.")]
    public ICloudInterfaceClient? Client;

    protected override void BeginProcessing()
    {
        base.BeginProcessing();

        if (Client == null)
        {
            Client = CloudXInterfacePool.Current ?? throw new InvalidOperationException("A client has not been established yet. Use Connect-NeosConnectApi to connect or provide a valid client using -Client.");
        }
    }

    protected override void ProcessRecord()
    {
        try
        {
            ExecuteCmdlet();
        }
        catch (PipelineStoppedException) { throw; }
        catch (Exception ex)
        {
            var errorMessage = ex.Message;

            if (!HasStoppingErrorAction())
            {
                throw new PSInvalidOperationException(errorMessage);
            }

            if (!HasIgnoreErrorAction())
            {
                ex.Data["TimeStampUtc"] = DateTime.UtcNow;

                var errDetails = new ErrorDetails(errorMessage);
                var errRecord = new ErrorRecord(ex, "EXCEPTION", ErrorCategory.WriteError, null);
                errRecord.ErrorDetails = errDetails;

                WriteError(errRecord);
            }
        }
    }
}
