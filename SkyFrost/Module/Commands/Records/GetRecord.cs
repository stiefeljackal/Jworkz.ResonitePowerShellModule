using System.Management.Automation;
using SkyFrost.Base;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Commands.Records;

using Commands.Abstract;
using Core.Utilities;

/// <summary>
/// Retrieves the record object from Resonite
/// </summary>
[Cmdlet(VerbsCommon.Get, "ResoniteRecord", DefaultParameterSetName = PARAM_SET_ASSETURI)]
public class GetRecord : ResoniteConnectedCmdlet
{
    private const string PARAM_SET_ASSETURI = "Get record by asset uri";

    /// <summary>
    /// Record id of the record object
    /// </summary>
    [ValidateNotNullOrEmpty]
    public string? RecordId;

    /// <summary>
    /// Record uri of the record object
    /// </summary>
    [Parameter(Mandatory = true, ParameterSetName = PARAM_SET_ASSETURI)]
    public Uri? RecordUri;

    protected override void ExecuteCmdlet()
    {
        Record record;

        RecordUri.ThrowOnInvalidPattern(CommonRegex.RecordUriRegex, $"The '{nameof(RecordUri)}' parameter does not match the resrec protocol.");
        record = Client!.GetRecord(RecordUri!).GetAwaiterResult();

        WriteObject(record);
    }
}