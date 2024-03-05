using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;

namespace Jworkz.ResonitePowerShellModule.Elements.Commands.DataTree
{
    using Elements.Commands.Abstract;
    using Elements.Models.DataTree;

    public class AddSlotNodeToParent : ResoniteSlotNodeCmdlet
    {
        /// <summary>
        /// The new parent slot for the target slot node
        /// </summary>
        [Parameter(Mandatory = true, Position = 1)]
        public SlotNode? ParentNode;

        protected override void ExecuteCmdlet()
        {
            base.ExecuteCmdlet();
        }
    }
}
