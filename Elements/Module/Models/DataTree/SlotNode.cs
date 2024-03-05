using Elements.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jworkz.ResonitePowerShellModule.Elements.Models.DataTree;

using Extensions;

public class SlotNode : SimpleDataTreeFamilyNode
{
    public Guid Id => new Guid(_dataTreeNode.GetValueFromKey<string>("ID") ?? string.Empty);

    public string? Name => _dataTreeNode.GetValueFromKey<string>("Name");

    public string Path => CreateStringPath().ToString();

    public ComponentNodeCollection Components => _dataTreeNode.GetListFromKey<DataTreeList>("Components")!.Select(c => new ComponentNode(c, this)).ToArray();

    public IEnumerable<SlotNode> ChildSlots => _dataTreeNode.GetListFromKey<DataTreeList>("Children")!.Select(s =>  new SlotNode(s, this)).ToArray();

    private DataTreeList ChildrenDataTreeList => (DataTreeList)_dataTreeNode["Children"];

    public SlotNode(DataTreeDictionary dictionary, SlotNode? parentNode = null) : base(dictionary, parentNode)
    {
    }

    public void AddChildSlotNode(SlotNode childSlotNode)
    {
        ChildrenDataTreeList.Children.Add(childSlotNode._dataTreeNode);
    }

    public bool RemoveChildSlotNode(SlotNode childSlotNode)
    {
        var hasRemoved = ChildrenDataTreeList.Children.Remove(childSlotNode._dataTreeNode);
        childSlotNode._dataTreeNode.Children.Remove("ParentReference");
        childSlotNode.ParentNode = null;

        return hasRemoved;
    }

    public SlotNode? this[string slotName]
    {
        get => ChildSlots.FirstOrDefault(s => s.Name == slotName);
    }

    public SlotNode? this[string slotName, int pos]
    {
        get => ChildSlots.Where(s => s.Name == slotName).ElementAtOrDefault(pos);
    }

    private StringBuilder CreateStringPath(StringBuilder? sb = null)
    {
        sb ??= new StringBuilder();

        if (ParentNode != null)
        {
            ParentNode.CreateStringPath(sb);
            sb.Append($" > {Name}");
        }
        else
        {
            sb.Append(Name);
        }

        return sb;
    }
}
