using System.Management.Automation;
using SkyFrost.Base;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.PipeBinds;

public class AssetInfoPipeBind
{
    public AssetInfo AssetInfo { get; }

    /// <summary>
    /// Hash id of the AssetInfo object
    /// </summary>
    public string HashId {
        get
        {
            var hashId = AssetInfo.AssetHash;

            if (string.IsNullOrEmpty(hashId))
            {
                throw new PSArgumentException("Hash id cannot be empty or null.");
            }
            return hashId;
        }
    }

    public AssetInfoPipeBind(string hashId)
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
}
