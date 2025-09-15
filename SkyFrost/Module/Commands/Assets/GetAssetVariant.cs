using System.Management.Automation;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Commands.Assets;

using Commands.Abstract;
using Core.Utilities;

/// <summary>
/// Retrieves all available variants for a specific asset from SkyFrost based on its hash id.
/// </summary>
[Cmdlet(VerbsCommon.Get, "SfAssetVariant")]
public class GetAssetVariant : SkyFrostAssetInfoCmdlet
{
    /// <summary>
    /// Executes the cmdlet to retrieve all available asset variants based on the specified hash id.
    /// </summary>
    protected override void ExecuteCmdlet()
    {
        var availableVariantsEnumerable = Client!.GetAvailableAssetVariants(HashId).GetAwaiterResult();

        WriteObject(availableVariantsEnumerable);
    }
}
