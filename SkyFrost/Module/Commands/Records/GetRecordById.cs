using System.Management.Automation;
using SkyFrost.Base;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Commands.Records;

using Commands.Abstract;
using Core.Utilities;
using SkyFrost.PipeBinds;

/// <summary>
/// Retrieves the record object by record id
/// </summary>
[Cmdlet(VerbsCommon.Get, "ResoniteRecordById")]
public class GetRecordById : ResoniteConnectedCmdlet
{
    /// <summary>
    /// Record id of the record object
    /// </summary>
    [Parameter(Mandatory = true, ValueFromPipeline = true)]
    public RecordIdPipeBind? RecordId;

    protected override void ExecuteCmdlet()
    {
        var recordIdValue = RecordId!.GetRecordId(Client);

        var record = Client!.GetRecord(recordIdValue!).GetAwaiterResult();

        WriteObject(record);
    }
}