using System.Management.Automation;

namespace Jworkz.ResonitePowerShellModule.Core.Commands.Assets;

using Commands.Abstract;
using Utilities;

/// <summary>
/// Retrieves the information of an asset from Resonite
/// </summary>
[Cmdlet(VerbsCommon.Get, "ResoniteAssetInfo", DefaultParameterSetName = PARAM_SET_GETCURRENTOWNERASSET)]
public class GetAssetInfo : ResoniteConnectedCmdlet
{
    private const string PARAM_SET_GETCURRENTOWNERASSET = "Get asset based on current user as owner";
    private const string PARAM_SET_GETOWNEDASSET = "Get asset based on Owned Route";
    private const string PARAM_SET_GETGLOBALASSET = "Get asset based on Global Route";
    
    /// <summary>
    /// Hash id of the asset info object
    /// </summary>
    [Parameter(Mandatory = true, ParameterSetName = PARAM_SET_GETCURRENTOWNERASSET, Position = 0)]
    [Parameter(Mandatory = true, ParameterSetName = PARAM_SET_GETOWNEDASSET, Position = 0)]
    [Parameter(Mandatory = true, ParameterSetName = PARAM_SET_GETGLOBALASSET, Position = 0)]
    public string HashId = string.Empty;

    /// <summary>
    /// Id of the owner of the asset
    /// </summary>
    [Parameter(Mandatory = false, ParameterSetName = PARAM_SET_GETOWNEDASSET)]
    public string? OwnerId;

    /// <summary>
    /// Retrieve the asset info with the current user as the owner
    /// </summary>
    [Parameter(Mandatory = true, ParameterSetName = PARAM_SET_GETGLOBALASSET, Position = 1)]
    public SwitchParameter Global;

    protected override void ExecuteCmdlet()
    {
        if (string.IsNullOrEmpty(HashId))
        {
            throw new PSArgumentException("Hash id cannot be empty or null");
        }

        var assetInfo = (
            Global.ToBool() ? Client?.GetGlobalAssetInfo(HashId!) :
            (string.IsNullOrEmpty(OwnerId) ? Client?.GetOwnedAssetInfo(HashId!)
            : Client?.GetOwnedAssetInfo(HashId!, OwnerId!))
        )?.GetAwaiterResult();

        WriteObject(assetInfo);
    }
}
