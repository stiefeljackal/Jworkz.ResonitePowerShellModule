using System.Management.Automation;

namespace Jworkz.ResonitePowerShellModule.Elements.Commands.Abstract;

using Core.Commands.Abstract;
using Elements.Models.DataTree;

/// <summary>
/// Base class for cmdlets that require a SlotNode object at the beginning position
/// or as a pipebind
/// </summary>
public class ResoniteSlotNodeCmdlet : BasePSCmdlet
{
    /// <summary>
    /// Main slot node to interact with
    /// </summary>
    [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
    public SlotNode? SlotNode;
}
