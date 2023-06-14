using JworkzNeosPowerShellModule.PipeBinds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace JworkzNeosPowerShellModule.Commands.Abstract
{
    /// <summary>
    /// Base class for cmdlets that require an AssetInfo object or hash id string
    /// </summary>
    public class NeosAssetInfoCmdlet : NeosConnectedCmdlet
    {
        /// <summary>
        /// File hash of the asset used as an id
        /// </summary>
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        public AssetInfoPipeBind AssetInfo = new AssetInfoPipeBind(string.Empty);

        /// <summary>
        /// Hash id of the assigned AssetInfo object
        /// </summary>
        [Parameter(ValueFromPipelineByPropertyName = true)]
        public string HashId => AssetInfo.HashId;
    }
}
