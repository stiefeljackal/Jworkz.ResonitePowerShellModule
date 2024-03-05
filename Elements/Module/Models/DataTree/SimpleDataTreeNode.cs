using Elements.Core;

namespace Jworkz.ResonitePowerShellModule.Elements.Models.DataTree;

public abstract class SimpleDataTreeNode
{
    protected readonly IDictionary<string, DataTreeNode> _dataKeyValuePairs;

    protected readonly DataTreeDictionary _dataTreeNode;


    public SimpleDataTreeNode(DataTreeDictionary dictionary)
    {
        _dataKeyValuePairs = dictionary.Children;
        _dataTreeNode = dictionary;
    }

    public DataTreeDictionary GetDataTreeDictionary() => _dataTreeNode;
}
