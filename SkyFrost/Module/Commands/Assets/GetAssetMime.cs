using System.Management.Automation;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Commands.Assets;

using Commands.Abstract;
using Core.Utilities;

/// <summary>
/// Retrives the mime type of the passed AssetInfo object or hash id
/// </summary>
[Cmdlet(VerbsCommon.Get, "ResoniteAssetMime")]
public class GetAssetMime : ResoniteAssetInfoCmdlet
{
    protected override void ExecuteCmdlet()
    {
        var hashId = HashId;
        var mimeType = Client!.GetAssetMime(hashId).GetAwaiterResult();

        WriteObject(mimeType);
    }
}
