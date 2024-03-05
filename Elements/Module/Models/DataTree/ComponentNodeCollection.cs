using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jworkz.ResonitePowerShellModule.Elements.Models.DataTree
{
    public class ComponentNodeCollection : IEnumerable<ComponentNode>
    {
        private IEnumerable<ComponentNode> _components;

        public int Count => _components.Count();

        public ComponentNodeCollection(IEnumerable<ComponentNode> enumerable)
        {
            _components = enumerable;
        }

        public IEnumerator<ComponentNode> GetEnumerator() => _components.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _components.GetEnumerator();

        public ComponentNode? this[int pos]
        {
            get => _components.ElementAtOrDefault(pos);
        }

        public ComponentNode? this[string componentType]
        {
            get => _components.FirstOrDefault(c => c.Type.Contains(componentType));
        }

        public ComponentNode? this[string componentType, int pos]
        {
            get => _components.Where(c => c.Type.Contains(componentType)).ElementAtOrDefault(pos);
        }

        public static implicit operator ComponentNodeCollection(List<ComponentNode> components) => new ComponentNodeCollection(components);

        public static implicit operator ComponentNodeCollection(ComponentNode[] components) => new ComponentNodeCollection(components);
    }
}
