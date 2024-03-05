using System.Management.Automation;

namespace Jworkz.ResonitePowerShellModule.Elements.Commands.DataTree;

using Elements.Models.DataTree;
using Elements.Commands.Abstract;

[Cmdlet(VerbsCommon.Find, "ResoniteComponentInDataTree", DefaultParameterSetName = PARAM_SET_FINDBYTYPE)]
public class FindComponentInDataTree : ResoniteSlotNodeCmdlet
{
    private const string PARAM_SET_FINDBYTYPE = "Find Component by Type";

    [Parameter(Mandatory = true, ParameterSetName = PARAM_SET_FINDBYTYPE)]
    public string? ComponentType;

    protected override void ExecuteCmdlet()
    {
        WriteObject((ComponentNodeCollection)FindComponents(SlotNode!).ToArray());
    }

    private IEnumerable<ComponentNode> FindComponents(SlotNode slot)
    {
        foreach(var component in slot.Components.Where(c => c.Type.Contains(ComponentType!)))
        {
            yield return component;
        }

        foreach(var childSlot in slot.ChildSlots)
        {
            foreach(var component in FindComponents(childSlot))
            {
                yield return component;
            }
        }
    }
}
