using Elements.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jworkz.ResonitePowerShellModule.Elements.Models.DataTree;

public class SimpleDataTreeRootNode : SimpleDataTreeNode
{
    public SlotNode RootSlot {  get; }

    public SimpleDataTreeRootNode(DataTreeDictionary dictionary) : base(dictionary)
    {
        var hasSlotRoot = _dataKeyValuePairs.TryGetValue("Slot", out var rootSlot)
            || _dataKeyValuePairs.TryGetValue("Slots", out rootSlot);

        if (!hasSlotRoot)
        {
            _dataKeyValuePairs.TryGetValue("Object", out rootSlot);
        }

        RootSlot = new SlotNode(rootSlot as DataTreeDictionary ?? throw new Exception("Unable to create a simple data tree root node."));
    }

    public static implicit operator SimpleDataTreeRootNode(DataTreeDictionary dictionary) => new SimpleDataTreeRootNode(dictionary);
}
