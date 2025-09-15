using System.Management.Automation;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Commands.Assets;

using Commands.Abstract;
using Core.Utilities;
using SkyFrost.PipeBinds;

/// <summary>
/// Retrieves information about an asset from SkyFrost based on its hash id or 
/// AssetInfo object.
/// </summary>
[Cmdlet(VerbsCommon.Get, "ResoniteAssetInfo", DefaultParameterSetName = PARAM_SET_GETOWNEDASSET)]
public class GetAssetInfo : ResoniteAssetInfoCmdlet
{
    private const string PARAM_SET_GETOWNEDASSET = "Without a pipebind, get asset based on Owned Route";
    private const string PARAM_SET_GETOWNEDASSETPIPEBIND = "With a pipebind, get asset based on Owned Route";
    private const string PARAM_SET_GETGLOBALASSET = "Without a pipebind, get asset based on Global Route";
    private const string PARAM_SET_GETGLOBALASSETPIPEBIND = "With a pipebind, get asset based on Global Route";

    /// <summary>
    /// // The asset information to retrieve. This can be specified as an AssetInfo object or a hash id.
    /// </summary>
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = PARAM_SET_GETOWNEDASSET)]
    [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = PARAM_SET_GETOWNEDASSETPIPEBIND)]
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = PARAM_SET_GETGLOBALASSET)]
    [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = PARAM_SET_GETGLOBALASSETPIPEBIND)]
    public override required AssetInfoPipeBind AssetInfo { get; set; }

    /// <summary>
    /// The identifier of the user or group that owns the asset.
    /// </summary>
    [Parameter(Mandatory = false, ParameterSetName = PARAM_SET_GETOWNEDASSET)]
    [Parameter(Mandatory = false, ParameterSetName = PARAM_SET_GETOWNEDASSETPIPEBIND)]
    public string? OwnerId;

    /// <summary>
    /// Switch that specifies the operation should target global assets.
    /// </summary>
    [Parameter(Mandatory = true, ParameterSetName = PARAM_SET_GETGLOBALASSET)]
    [Parameter(Mandatory = true, ParameterSetName = PARAM_SET_GETGLOBALASSETPIPEBIND)]
    public SwitchParameter Global;

    /// <summary>
    /// Executes the cmdlet to retrieve asset information based on the specified parameters.
    /// </summary>
    protected override void ExecuteCmdlet()
    {
        var foundAssetInfo = Global
            ? Client!.GetGlobalAssetInfo(HashId).GetAwaiterResult()
            : Client!.GetOwnedAssetInfo(HashId, OwnerId).GetAwaiterResult();

        WriteObject(foundAssetInfo);
    }
}
