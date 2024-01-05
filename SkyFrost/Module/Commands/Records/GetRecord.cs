using System.Management.Automation;
using SkyFrost.Base;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Commands.Records;

using Commands.Abstract;
using Core.Utilities;

/// <summary>
/// Retrieves the record object from Resonite
/// </summary>
[Cmdlet(VerbsCommon.Get, "ResoniteRecord", DefaultParameterSetName = PARAM_SET_CURRENTUSERRECORD)]
public class GetRecord : ResoniteConnectedCmdlet
{
    private const string PARAM_SET_CURRENTUSERRECORD = "Get record by current user as owner";
    private const string PARAM_SET_OWNERRECORD = "Get record by owner id";
    private const string PARAM_SET_ASSETURI = "Get record by asset uri";

    /// <summary>
    /// Owner id of the owned record object
    /// </summary>
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = PARAM_SET_OWNERRECORD)]
    [ValidateNotNullOrEmpty]
    public string? OwnerId;

    /// <summary>
    /// Record id of the record object
    /// </summary>
    [Parameter(Mandatory = true, Position = 1, ParameterSetName = PARAM_SET_OWNERRECORD)]
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = PARAM_SET_CURRENTUSERRECORD)]
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

        if (ParameterSetName == PARAM_SET_ASSETURI)
        {
            RecordUri.ThrowOnInvalidPattern(CommonRegex.RecordUriRegex, $"The '{nameof(RecordUri)}' parameter does not match the resrec protocol.");
            record = Client!.GetRecord(RecordUri!).GetAwaiterResult();
        }
        else
        {
            if (ParameterSetName == PARAM_SET_CURRENTUSERRECORD)
            {
                OwnerId = Client?.CurrentUser.Id ?? string.Empty;
            }
            record = Client!.GetRecord(OwnerId!, RecordId!).GetAwaiterResult();
        }

        WriteObject(record);
    }
}