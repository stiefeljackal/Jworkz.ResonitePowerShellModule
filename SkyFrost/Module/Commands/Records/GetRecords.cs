using System.Management.Automation;
using SkyFrost.Base;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Commands.Records;

using Commands.Abstract;
using Core.Utilities;

/// <summary>
/// Retrieves the record object from Resonite
/// </summary>
[Cmdlet(VerbsCommon.Get, "ResoniteRecords", DefaultParameterSetName = PARAM_SET_CURRENTUSERRECORDFROMPATH)]
public class GetRecords : ResoniteConnectedCmdlet
{
    private const string PARAM_SET_BATCHRECORD = "Get multiple Records from batch";
    private const string PARAM_SET_RECORDFROMPATH = "Get multiple Records from path";
    private const string PARAM_SET_CURRENTUSERBATCHRECORD = "Get the Current User's Records from batch";
    private const string PARAM_SET_CURRENTUSERRECORDFROMPATH = "Get the Current User's Records from path";

    [Parameter(Mandatory = true, Position = 0, ParameterSetName = PARAM_SET_BATCHRECORD)]
    [ValidateNotNull]
    public RecordId[] BatchRecordIdCollection = new RecordId[0];

    [Parameter(Mandatory = true, Position = 0, ParameterSetName = PARAM_SET_CURRENTUSERBATCHRECORD)]
    [ValidateNotNull]
    public string[] BatchRecordIdCollectionForCurrentUser = new string[0];

    /// <summary>
    /// Owner id of the owned record object
    /// </summary>
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = PARAM_SET_RECORDFROMPATH)]
    [ValidateNotNullOrEmpty]
    public string OwnerId = string.Empty;

    [Parameter(Mandatory = true, Position = 1, ParameterSetName = PARAM_SET_RECORDFROMPATH)]
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = PARAM_SET_CURRENTUSERRECORDFROMPATH)]
    [ValidateNotNull]
    public string Path = string.Empty;

    protected override void ExecuteCmdlet()
    {
        IEnumerable<Record>? records;

        switch (ParameterSetName)
        {
            case PARAM_SET_BATCHRECORD:
                records = Client?.GetRecords(BatchRecordIdCollection).GetAwaiterResult();
                break;
            default:
                var currentUserId = Client?.CurrentUser.Id;
                records = Client?.GetRecords(BatchRecordIdCollectionForCurrentUser.Select(r => new RecordId { Id = r, OwnerId = currentUserId })).GetAwaiterResult();
                break;
        }

        WriteObject(records ?? new Record[0]);
    }
}
