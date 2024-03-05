using Elements.Core;

namespace Jworkz.ResonitePowerShellModule.Elements.Models.DataTree;

public abstract class SimpleDataTreeFamilyNode : SimpleDataTreeNode
{
    public SlotNode? ParentNode { get; protected set; }
    
    public SimpleDataTreeFamilyNode(DataTreeDictionary dictionary, SlotNode? parentNode) : base(dictionary)
    {
        ParentNode = parentNode;
    }
}
