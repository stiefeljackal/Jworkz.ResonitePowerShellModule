using CloudX.Shared;
using JworkzNeosPowerShellModule.Commands.Abstract;
using JworkzNeosPowerShellModule.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace JworkzNeosPowerShellModule.Commands.Assets;

/// <summary>
/// Retrieves the information of an asset from NeosVR
/// </summary>
[Cmdlet(VerbsCommon.Get, "NeosAssetInfo", DefaultParameterSetName = PARAM_SET_GETGLOBALASSET)]
public class GetAssetInfo : NeosConnectedCmdlet
{
    private const string PARAM_SET_GETOWNEDASSET = "Get asset based on Owned Route";
    private const string PARAM_SET_GETCURRENTOWNERASSET = "Get asset based on current user as owner";
    private const string PARAM_SET_GETGLOBALASSET = "Get asset based on Global Route";
    
    /// <summary>
    /// Hash id of the asset info object
    /// </summary>
    [Parameter(Mandatory = true, ParameterSetName = PARAM_SET_GETOWNEDASSET, Position = 0)]
    [Parameter(Mandatory = true, ParameterSetName = PARAM_SET_GETGLOBALASSET, Position = 0)]
    [Parameter(Mandatory = true, ParameterSetName = PARAM_SET_GETCURRENTOWNERASSET, Position = 0)]
    public string HashId = string.Empty;

    /// <summary>
    /// Id of the owner of the asset
    /// </summary>
    [Parameter(Mandatory = false, ParameterSetName = PARAM_SET_GETOWNEDASSET)]
    public string? OwnerId;

    /// <summary>
    /// Retrieve the asset info with the current user as the owner
    /// </summary>
    [Parameter(Mandatory = true, ParameterSetName = PARAM_SET_GETCURRENTOWNERASSET)]
    public SwitchParameter WithCurrentUserAsOwner;

    protected override void ExecuteCmdlet()
    {
        if (string.IsNullOrEmpty(HashId))
        {
            throw new PSArgumentException("Hash id cannot be empty or null");
        }

        var assetInfo = (
            WithCurrentUserAsOwner.ToBool() ? Client?.GetOwnedAssetInfo(HashId) :
            (string.IsNullOrEmpty(OwnerId) ? Client?.GetGlobalAssetInfo(HashId!)
            : Client?.GetOwnedAssetInfo(HashId!, OwnerId!))
        )?.GetAwaiterResult();

        WriteObject(assetInfo);
    }
}
