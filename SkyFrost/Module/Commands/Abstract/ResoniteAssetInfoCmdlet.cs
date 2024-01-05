using System.Management.Automation;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Commands.Abstract;

using PipeBinds;

/// <summary>
/// Base class for cmdlets that require an AssetInfo object or hash id string
/// </summary>
public class ResoniteAssetInfoCmdlet : ResoniteConnectedCmdlet
{
    /// <summary>
    /// File hash of the asset used as an id
    /// </summary>
    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
    public AssetInfoPipeBind AssetInfo = new AssetInfoPipeBind(string.Empty);

    /// <summary>
    /// Hash id of the assigned AssetInfo object
    /// </summary>
    [Parameter(ValueFromPipelineByPropertyName = true)]
    public string HashId => AssetInfo.HashId;
}
