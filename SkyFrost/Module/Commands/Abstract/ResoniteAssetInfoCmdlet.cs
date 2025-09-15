using System.Management.Automation;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Commands.Abstract;

using PipeBinds;
using Core.Models.Abstract;

/// <summary>
/// Base class for cmdlets that require an AssetInfo object or hash id string.
/// </summary>
public abstract class ResoniteAssetInfoCmdlet : ResoniteConnectedCmdlet
{
    /// <summary>
    /// AssetInfo object or hash id string representing the asset.
    /// </summary>
    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
    public virtual required AssetInfoPipeBind AssetInfo { get; set; }

    /// <summary>
    /// Gets the unique hash identifier associated with the asset.
    /// </summary>
    public string HashId => AssetInfo.HashId;

    public ResoniteAssetInfoCmdlet() : base() { }

    public ResoniteAssetInfoCmdlet(IFileSystem fileSystem) : base(fileSystem) { }

    /// <summary>
    /// Prepares the cmdlet for execution by performing necessary validation and setup.
    /// </summary>
    /// <remarks>This method ensures that the required <see cref="HashId"/> property is set before the cmdlet
    /// is executed.</remarks>
    /// <exception cref="InvalidOperationException">Thrown if the <see cref="HashId"/> property is null or an empty string.</exception>
    protected override void PrepareCmdlet()
    {
        base.PrepareCmdlet();

        if (string.IsNullOrEmpty(HashId))
        {
            throw new InvalidOperationException("The asset hash id cannot be null or empty.");
        }
    }
}
