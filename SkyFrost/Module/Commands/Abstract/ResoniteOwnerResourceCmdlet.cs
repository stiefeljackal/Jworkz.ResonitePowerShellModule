using System.Management.Automation;
using OwnerTypeEnum = SkyFrost.Base.OwnerType;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Commands.Abstract;

using PipeBinds;

public abstract class ResoniteOwnerResourceCmdlet : ResoniteConnectedCmdlet
{
    /// <summary>
    /// Owner of the resource
    /// </summary>
    public virtual OwnerPipeBind? Owner { get; set; }

    /// <summary>
    /// Id of the owner
    /// </summary>
    public string OwnerId => Owner?.OwnerId ?? string.Empty;

    /// <summary>
    /// User type of the owner
    /// </summary>
    public OwnerTypeEnum OwnerType => Owner?.OwnerType ?? OwnerTypeEnum.INVALID;

    /// <summary>
    /// Ensure the owner is either a user or group type
    /// </summary>
    /// <exception cref="PSInvalidOperationException"></exception>
    protected override void ExecuteCmdlet()
    {
        base.ExecuteCmdlet();

        if (Owner == null || !Owner.IsValidOwnerId())
        {
            throw new PSInvalidOperationException($"OwnerId '{OwnerId}' is not valid.");
        }
    }
}
