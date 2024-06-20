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
    private const string PARAM_SET_GETEXACTRECORDATPATH = "Get exact record at path";

    /// <summary>
    /// Owner of the resource
    /// </summary>
    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = PARAM_SET_RECORDSPATHFILTER)]
    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = PARAM_SET_RECORDSTAGFILTER)]
    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ParameterSetName = PARAM_SET_GETEXACTRECORDATPATH)]
    public override OwnerPipeBind? Owner { get; set; }

    /// <summary>
    /// Optional path filter of where the records reside in
    /// </summary>
    [Parameter(Position = 1, Mandatory = false, ParameterSetName = PARAM_SET_RECORDSPATHFILTER)]
    [Parameter(Position = 1, Mandatory = true, ParameterSetName = PARAM_SET_GETEXACTRECORDATPATH)]
    public string? Path;

    /// <summary>
    /// Optional tag to filter records by
    /// </summary>
    [Parameter(Position = 1, Mandatory = false, ParameterSetName = PARAM_SET_RECORDSTAGFILTER)]
    public string? Tag;

    /// <summary>
    /// Switch that determines if the provided path is the exact path to the individual record
    /// </summary>
    [Parameter(Position = 2, Mandatory = true, ParameterSetName = PARAM_SET_GETEXACTRECORDATPATH)]
    public SwitchParameter AsExactRecordPath;

    /// <summary>
    /// Optional access key used to access a record that is normally private
    /// </summary>
    [Parameter(ParameterSetName = PARAM_SET_GETEXACTRECORDATPATH)]
    public string? AccessKey;

    protected override void ExecuteCmdlet()
    {
        base.ExecuteCmdlet();

        if (AsExactRecordPath.IsPresent)
        {
            var record = Client!.GetRecordAtPath(OwnerId, Path!, AccessKey).GetAwaiterResult();
            WriteObject(record);
            return;
        }

        var records = Client!.GetRecordsByOwner(OwnerId, Tag, Path).GetAwaiterResult();
        foreach (var record in records)
        {
            WriteObject(record);
        }
    }
}
