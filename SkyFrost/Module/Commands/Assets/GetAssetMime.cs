using System.Management.Automation;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Commands.Assets;

using Commands.Abstract;
using Core.Utilities;

/// <summary>
/// Retrieves the MIME type of an asset from SkyFrost based on its hash id.
/// </summary>
[Cmdlet(VerbsCommon.Get, "SfAssetMime")]
public class GetAssetMime : SkyFrostAssetInfoCmdlet
{
    /// <summary>
    /// Executes the cmdlet to retrieve the MIME type of the asset based on the specified hash id.
    /// </summary>
    protected override void ExecuteCmdlet()
    {
        var mimeType = Client!.GetAssetMime(HashId).GetAwaiterResult();
        WriteObject(mimeType);
    }
}
