using System.Management.Automation;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Commands.Records;

using Abstract;
using Core.Utilities;

public class GetOwnRecord : ResoniteOwnerResourceCmdlet
{
    [Parameter(Mandatory = true, Position = 1)]
    [Alias("RecordId")]
    public string? RecordPostfixId;

    protected override void PrepareCmdlet()
    {
        base.PrepareCmdlet();
        RecordPostfixId.ThrowOnInvalidPattern(CommonRegex.RecordPostfixIdRegex, $"The 'RecordId' does not match the 'R-' pattern.");
    }

    protected override void ExecuteCmdlet()
    {
        var record = Client!.GetRecord(OwnerId, RecordPostfixId!).GetAwaiterResult();
        WriteObject(record);
    }
}
