using CloudX.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace JworkzNeosPowerShellModule.PipeBinds;

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
