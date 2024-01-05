using System.Management.Automation;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Commands.Assets;

using Commands.Abstract;
using Core.Utilities;

/// <summary>
/// Retrieves the asset variants that the cloud has registered.
/// </summary>
[Cmdlet(VerbsCommon.Get, "ResoniteAvailableAssetVariants")]
public class GetAvailableAssetVariants : ResoniteAssetInfoCmdlet
{
    protected override void ExecuteCmdlet()
    {
        var availableVariantsEnumerable = Client!.GetAvailableAssetVariants(HashId).GetAwaiterResult();

        WriteObject(availableVariantsEnumerable);
    }
}
