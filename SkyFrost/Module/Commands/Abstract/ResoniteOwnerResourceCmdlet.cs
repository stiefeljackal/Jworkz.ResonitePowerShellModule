using System.Management.Automation;
using SkyFrost.Base;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Commands.Abstract;

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
    public string OwnerId => Owner?.OwnerId ?? string.Empty;

    /// <summary>
    /// Ensure the owner is either a user or group type
    /// </summary>
    /// <exception cref="PSInvalidOperationException"></exception>
    protected override void PrepareCmdlet()
    {
        base.PrepareCmdlet();

        var ownerType = Owner?.OwnerType ?? OwnerType.INVALID;

        if (ownerType == OwnerType.INVALID || ownerType == OwnerType.Machine)
        {
            throw new PSInvalidOperationException($"OwnerId '{OwnerId}' is not valid.");
        }
    }
}
