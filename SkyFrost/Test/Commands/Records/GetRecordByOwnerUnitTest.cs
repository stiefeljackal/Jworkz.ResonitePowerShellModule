using Moq;
using SkyFrost.Base;

using SkyFrostRecord = SkyFrost.Base.Record;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Test.Commands.Records;

using SkyFrost.Commands.Records;
using SkyFrost.PipeBinds;
using SkyFrost.Clients.Abstract;

public class GetRecordByOwnerUnitTest
{
    [Theory]
    [MemberData(nameof(OwnerData))]
    public void ExecuteCmdlet_ValidOwner_ReturnsArrayOfRecords(OwnerPipeBind ownerMock)
    {
        IEnumerable<SkyFrostRecord> recordsMock = GetRecordsMock(ownerMock.OwnerId);
        MockCommandRuntime<SkyFrostRecord> runtime = new();
        Mock<ISkyFrostInterfaceClient> skyFrostClientMock = new();

        skyFrostClientMock
            .Setup(m => m.GetRecordsByOwner(It.Is<string>(arg => arg == ownerMock.OwnerId), It.Is<string>(arg => arg == null), It.Is<string>(arg => arg == null)))
            .Returns(() => Task.FromResult(recordsMock));

        GetRecordByOwner cmdlet = new()
        {
            CommandRuntime = runtime,
            Owner = ownerMock,
            Client = skyFrostClientMock.Object
        };

        cmdlet.StartProcessExecution();

        Assert.Equal(ownerMock.OwnerId, cmdlet.OwnerId);
        Assert.Equal(ownerMock.OwnerType, cmdlet.OwnerType);
        Assert.Equal(recordsMock, runtime.Output);
    }

    /*[Theory]
    [MemberData(nameof(InvalidOwnerData))]
    public void ExecuteCmdlet_InvalidOwner_ThrowsError()
    {
        IEnumerable<SkyFrostRecord> recordsMock = GetRecordsMock(ownerMock.OwnerId);
        MockCommandRuntime<SkyFrostRecord> runtime = new();
        Mock<ISkyFrostInterfaceClient> mockSkyFrostClient = new();

        mockSkyFrostClient
            .Setup(m => m.GetRecordsByOwner(It.Is<string>(arg => arg == ownerMock.OwnerId), It.Is<string>(arg => arg == null), It.Is<string>(arg => arg == null)))
            .Returns(() => Task.FromResult(recordsMock));

        GetRecordByOwner cmdlet = new()
        {
            CommandRuntime = runtime,
            Owner = ownerMock,
            Client = mockSkyFrostClient.Object
        };

        cmdlet.StartProcessExecution();

        Assert.Equal(ownerMock.OwnerId, cmdlet.OwnerId);
        Assert.Equal(ownerMock.OwnerType, cmdlet.OwnerType);
        Assert.Equal(recordsMock, runtime.Output);
    }*/

    private static IEnumerable<SkyFrostRecord> GetRecordsMock(string ownerId)
    {
        SkyFrostRecord[] records = new SkyFrostRecord[10];

        Array.Fill(records, new()
        {
            OwnerId = ownerId,
            RecordId = GlobalConstants.MOCK_RECORD_ID
        });

        return records;
    }

    public static IEnumerable<object[]> OwnerData =>
        new object[][]
        {
            new [] { new User { Id = GlobalConstants.MOCK_USER_ID } },
            new [] { new Group { GroupId = GlobalConstants.MOCK_GROUP_ID } },
            new [] { GlobalConstants.MOCK_USER_ID },
            new [] { GlobalConstants.MOCK_GROUP_ID }
        };

    public static IEnumerable<object[]> InvalidOwnerData =>
        new object[][]
        {
            new [] { new User { Id = GlobalConstants.MOCK_MACHINE_ID } },
            new [] { new OwnerPipeBind(string.Empty) }
        };
}
