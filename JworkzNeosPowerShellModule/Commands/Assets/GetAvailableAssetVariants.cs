using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;

namespace JworkzNeosPowerShellModule.Commands.Assets;

using CodeX;
using Commands.Abstract;
using Utilities;

/// <summary>
/// Retrieves the asset variants that the cloud has registered.
/// </summary>
[Cmdlet(VerbsCommon.Get, "NeosAvailableAssetVariants")]
public class GetAvailableAssetVariants : NeosAssetInfoCmdlet
{
    protected override void ExecuteCmdlet()
    {
        var availableVariants = Client!.GetAvailableAssetVariants(HashId).GetAwaiterResult();

        WriteObject(availableVariants);
    }
}
