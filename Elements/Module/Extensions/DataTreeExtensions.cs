using Elements.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jworkz.ResonitePowerShellModule.Elements.Extensions;

public static class DataTreeExtensions
{
    public static T? GetValueFromKey<T>(this DataTreeDictionary dtDictionary, string key)
    {
        var dtProperty = dtDictionary[key];
        var dtData = dtProperty is DataTreeDictionary ? ((DataTreeDictionary)dtProperty)["Data"] as DataTreeValue : dtProperty as DataTreeValue;

        return (T?)dtData?.Value ?? default;
    }

    public static IEnumerable<DataTreeDictionary> GetListFromKey<T>(this DataTreeDictionary dtDictionary, string key)
    {
        new int();
        var dtProperty = dtDictionary[key];
        var dtData = dtProperty is DataTreeDictionary ? ((DataTreeDictionary)dtProperty)?["Data"] as DataTreeList : dtProperty as DataTreeList;

        return dtData?.Cast<DataTreeDictionary>() ?? Array.Empty<DataTreeDictionary>();
    }
}
