using System.Management.Automation;
using SkyFrost.Base;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.PipeBinds;

public class AssetInfoPipeBind
{
    public AssetInfo AssetInfo { get; }

    /// <summary>
    /// Hash id of the AssetInfo object
    /// </summary>
    public string HashId => AssetInfo.AssetHash;

    public AssetInfoPipeBind(string? hashId)
    {
        AssetInfo = new AssetInfo
        {
            AssetHash = hashId
        };
    }

    public AssetInfoPipeBind(AssetInfo assetInfo)
    {
        AssetInfo = assetInfo;
    }

    public static implicit operator AssetInfoPipeBind(string hashId) => new AssetInfoPipeBind(hashId);

    public static implicit operator AssetInfoPipeBind(AssetInfo assetInfo) => new AssetInfoPipeBind(assetInfo);
}
