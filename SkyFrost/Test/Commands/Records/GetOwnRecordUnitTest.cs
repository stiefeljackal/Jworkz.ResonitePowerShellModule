using Moq;

using SkyFrostRecord = SkyFrost.Base.Record;

namespace Jworkz.ResonitePowerShellModule.Core.Test.Commands.Records;

using SkyFrost.Commands.Records;
using SkyFrost.PipeBinds;
using SkyFrost.Clients.Abstract;

public class GetOwnRecordUnitTest
{
    private const string OWNER_ID = "U-Mock";

    [Fact]
    public void ExecuteCmdlet_ValidRecordId_ReturnsRecord()
    {
        var recordId = "R-00000000-0000-4000-0000-000000000000";
        var record = new SkyFrostRecord
        {
            OwnerId = OWNER_ID,
            RecordId = recordId,
        };

        MockCommandRuntime<SkyFrostRecord> runtime = new();
        OwnerPipeBind mockOwnerPipeBind = new(OWNER_ID);
        Mock<ISkyFrostInterfaceClient> mockSkyFrostClient = new();

        mockSkyFrostClient
            .Setup(m => m.GetRecord(It.Is<string>(arg => arg == OWNER_ID), It.Is<string>(arg => arg == recordId)))
            .Returns(() => Task.FromResult(record));

        GetOwnRecord cmdlet = new()
        {
            CommandRuntime = runtime,
            Owner = mockOwnerPipeBind,
            RecordPostfixId = recordId,
            Client = mockSkyFrostClient.Object
        };

        cmdlet.StartProcessExecution();

        Assert.Single(runtime.Output);
        Assert.Equal(record, runtime.Output.First());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData("R-abc-123")]
    public void ExecuteCmdlet_InvalidRecordId_ReturnsError(string? recordId)
    {
        MockCommandRuntime<SkyFrostRecord> runtime = new();
        OwnerPipeBind mockOwnerPipeBind = new(OWNER_ID);
        Mock<ISkyFrostInterfaceClient> mockSkyFrostClient = new();

        GetOwnRecord cmdlet = new()
        {
            CommandRuntime = runtime,
            Owner = mockOwnerPipeBind,
            RecordPostfixId = recordId,
            Client = mockSkyFrostClient.Object,
        };

        cmdlet.MyInvocation.BoundParameters.Add("ErrorAction", "Stop");
        cmdlet.StartProcessExecution();

        Assert.Single(runtime.Errors);
    }
}
