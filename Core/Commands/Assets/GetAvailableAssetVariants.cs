using System.Management.Automation;

namespace Jworkz.ResonitePowerShellModule.Core.Commands.Assets;

using Commands.Abstract;
using Utilities;

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
