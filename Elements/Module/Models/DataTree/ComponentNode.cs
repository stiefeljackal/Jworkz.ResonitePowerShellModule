using Elements.Core;

namespace Jworkz.ResonitePowerShellModule.Elements.Models.DataTree;

using Extensions;

public class ComponentNode : SimpleDataTreeFamilyNode
{
    public string Type => _dataTreeNode.GetValueFromKey<string>("Type")!;

    public ComponentNode(DataTreeDictionary dictionary, SlotNode parentNode) : base(dictionary, parentNode)
    {
    }
}
