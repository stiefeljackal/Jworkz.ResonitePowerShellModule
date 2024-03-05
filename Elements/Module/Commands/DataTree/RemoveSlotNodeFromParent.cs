using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;

namespace Jworkz.ResonitePowerShellModule.Elements.Commands.DataTree;

using Elements.Commands.Abstract;

public class RemoveSlotNodeFromParent : ResoniteSlotNodeCmdlet
{
    protected override void ExecuteCmdlet()
    {
        var parentNode = SlotNode!.ParentNode;
        if (parentNode == null) { return; }

        parentNode.RemoveChildSlotNode(SlotNode);
    }
}
