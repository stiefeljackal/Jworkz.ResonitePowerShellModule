using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace JworkzNeosPowerShellModule.Commands.Assets;

using Commands.Abstract;
using JworkzNeosPowerShellModule.Utilities;
using PipeBinds;

/// <summary>
/// Retrives the mime type of the passed AssetInfo object or hash id
/// </summary>
[Cmdlet(VerbsCommon.Get, "NeosAssetMime")]
public class GetAssetMime : NeosAssetInfoCmdlet
{
    protected override void ExecuteCmdlet()
    {
        var hashId = HashId;
        var mimeType = Client!.GetAssetMime(hashId).GetAwaiterResult();

        WriteObject(mimeType);
    }
}
