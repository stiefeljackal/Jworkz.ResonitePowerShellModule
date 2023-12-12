using System.Management.Automation;

namespace Jworkz.ResonitePowerShellModule.Core.Commands.Abstract;

/// <summary>
/// Base class for all cmdlets
/// </summary>
public class BasePSCmdlet : PSCmdlet
{
    /// <summary>
    /// Error action specified for this cmdlet if called
    /// </summary>
    public string? ErrorActionSpecified
    {
        get => MyInvocation.BoundParameters["ErrorAction"].ToString()?.ToLowerInvariant();
    }

    protected override void BeginProcessing()
    {
        base.BeginProcessing();
    }

    protected override void EndProcessing()
    {
        base.EndProcessing();
    }

    /// <summary>
    /// Checks if a parameter with the provided name has been provided in the execution command
    /// </summary>
    /// <param name="paramName">Name of the parameter to validate if it has been provided in the execution command</param>
    /// <returns>True if a parameter with the provided name is present, false if it is not</returns>
    protected bool IsParamSpecified(string paramName)
    {
        return MyInvocation.BoundParameters.ContainsKey(paramName);
    }

    protected bool HasStoppingErrorAction()
    {
        return IsParamSpecified("ErrorAction") && (new[] { "stop", "ignore", "silentlycontinue" }).Contains(ErrorActionSpecified);
    }

    protected bool HasIgnoreErrorAction()
    {
        return IsParamSpecified("ErrorAction") && ErrorActionSpecified == "ignore";
    }

    protected virtual void ExecuteCmdlet() { }

    protected override void ProcessRecord()
    {
        try
        {
            ExecuteCmdlet();
        }
        catch(Exception ex)
        {
            throw new PSInvalidOperationException(ex.Message);
        }
    }

    protected override void StopProcessing()
    {
        base.StopProcessing();
    }

    internal void WriteError(Exception ex, ErrorCategory errorCategory, object? target = null) 
    {
        this.WriteError(new ErrorRecord(ex, string.Empty, errorCategory, target));
    }
}
