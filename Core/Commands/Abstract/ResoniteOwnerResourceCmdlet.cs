using System.Management.Automation;
using SkyFrost.Base;

namespace Jworkz.ResonitePowerShellModule.Core.Commands.Abstract;

using PipeBinds;

public class ResoniteOwnerResourceCmdlet : ResoniteConnectedCmdlet
{
    /// <summary>
    /// Owner of the resource
    /// </summary>
    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
    public OwnerPipeBind? Owner;
    
    /// <summary>
    /// Id of the owner
    /// </summary>
    [Parameter(ValueFromPipelineByPropertyName = true)]
    public string OwnerId = string.Empty;

    /// <summary>
    /// Declared type of the owner
    /// </summary>
    public OwnerType OwnerType => IdUtil.GetOwnerType(OwnerId);

    /// <summary>
    /// Ensure the owner is either a user or group type
    /// </summary>
    /// <exception cref="PSInvalidOperationException"></exception>
    protected override void BeginProcessing()
    {
        var ownerType = OwnerType;
        base.BeginProcessing();
        if (ownerType == OwnerType.INVALID || ownerType == OwnerType.Machine)
        {
            throw new PSInvalidOperationException($"OwnerId '{OwnerId}' is not valid.");
        }
    }
}
