using System.Management.Automation;
using SkyFrost.Base;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Commands.Records;

using Abstract;
using Core.Utilities;
using PipeBinds;

/// <summary>
/// Retrieves records associated by the Owner
/// </summary>
[Cmdlet(VerbsCommon.Get, "ResoniteRecordByOwner", DefaultParameterSetName = PARAM_SET_RECORDSPATHFILTER)]
[OutputType(typeof(Record))]
public class GetRecordByOwner : ResoniteOwnerResourceCmdlet
{
    private const string PARAM_SET_RECORDSPATHFILTER = "Filter records by path";
    private const string PARAM_SET_RECORDSTAGFILTER = "Filter records by a tag";

    /// <summary>
    /// Owner of the resource
    /// </summary>
    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = PARAM_SET_RECORDSPATHFILTER)]
    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = PARAM_SET_RECORDSTAGFILTER)]
    public override OwnerPipeBind? Owner { get; set; }

    /// <summary>
    /// Optional path filter of where the records reside in
    /// </summary>
    [Parameter(Position = 1, Mandatory = false, ParameterSetName = PARAM_SET_RECORDSPATHFILTER)]
    public string? Path;

    /// <summary>
    /// Optional tag to filter records by
    /// </summary>
    [Parameter(Position = 1, Mandatory = false, ParameterSetName = PARAM_SET_RECORDSTAGFILTER)]
    public string? Tag;

    protected override void ExecuteCmdlet()
    {
        base.ExecuteCmdlet();

        var records = Client!.GetRecordsByOwner(OwnerId, Tag, Path).GetAwaiterResult();
        foreach (var record in records)
        {
            WriteObject(record);
        }
    }
}
