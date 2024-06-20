using System.Diagnostics.CodeAnalysis;
using Moq;
using SkyFrost.Base;

using SkyFrostRecord = SkyFrost.Base.Record;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Test.Commands.Records;

using SkyFrost.Commands.Records;
using SkyFrost.PipeBinds;
using SkyFrost.Clients.Abstract;

[ExcludeFromCodeCoverage]
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

    [Fact]
    public void ExecuteCmdlet_ExactRecordPath_ReturnsRecordAtPath()
    {
        MockCommandRuntime<SkyFrostRecord> runtime = new();
        Mock<ISkyFrostInterfaceClient> skyFrostClientMock = new();

        var mockRecord = new SkyFrostRecord();
        var mockPath = @"Inventory\Mock";

        skyFrostClientMock
            .Setup(m => m.GetRecordAtPath(It.Is<string>(arg => arg == GlobalConstants.MOCK_USER_ID), It.Is<string>(arg => arg == mockPath), It.Is<string>(arg => arg == null)))
            .Returns(() => Task.FromResult(mockRecord));

        GetRecordByOwner cmdlet = new()
        {
            CommandRuntime = runtime,
            Owner = new OwnerPipeBind(GlobalConstants.MOCK_USER_ID),
            Path = mockPath,
            Client = skyFrostClientMock.Object,
            AsExactRecordPath = true
        };

        cmdlet.StartProcessExecution();

        skyFrostClientMock.Verify(m => m.GetRecordAtPath(It.Is<string>(arg => arg == GlobalConstants.MOCK_USER_ID), It.Is<string>(arg => arg == mockPath), It.Is<string>(arg => arg == null)), Times.Once);

        Assert.Equal(mockRecord, runtime.Output.First());
        Assert.Single(runtime.Output);
    }

    [Fact]
    public void ExecuteCmdlet_IncludeHierarchySwitch_CallsGetRecordsInHierarchyMethod()
    {
        MockCommandRuntime<SkyFrostRecord> runtime = new();
        Mock<ISkyFrostInterfaceClient> mockSkyFrostClient = new();
        Mock<IAsyncEnumerable<SkyFrostRecord>> mockAsyncEnumerable = new();
        Mock<IAsyncEnumerator<SkyFrostRecord>> mockAsyncEnumerator = new();

        var mockCurrentIndex = -1;
        var mockRecords = new[] {
            new SkyFrostRecord(),
            new SkyFrostRecord(),
            new SkyFrostRecord()
        };
        var mockPath = @"Inventory\Mock";

        mockSkyFrostClient
            .Setup(m => m.GetRecordsInHierarchy(It.Is<string>(arg => arg == GlobalConstants.MOCK_USER_ID), It.Is<string>(arg => arg == mockPath)))
            .Returns(() => mockAsyncEnumerable.Object);
        mockAsyncEnumerable
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(() => mockAsyncEnumerator.Object);
        mockAsyncEnumerator
            .Setup(m => m.Current)
            .Returns(() => mockRecords[mockCurrentIndex]);
        mockAsyncEnumerator
            .Setup(m => m.MoveNextAsync())
            .Returns(() => new ValueTask<bool>(++mockCurrentIndex < mockRecords.Length));

        GetRecordByOwner cmdlet = new()
        {
            CommandRuntime = runtime,
            Owner = new OwnerPipeBind(GlobalConstants.MOCK_USER_ID),
            Path = mockPath,
            Client = mockSkyFrostClient.Object,
            IncludeHierarchy = true
        };

        cmdlet.StartProcessExecution();

        mockSkyFrostClient.Verify(m => m.GetRecordsInHierarchy(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        mockAsyncEnumerable.Verify(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()), Times.Once());
        mockAsyncEnumerator.Verify(m => m.MoveNextAsync(), Times.Exactly(mockRecords.Length + 1));

        Assert.Equal(mockRecords, runtime.Output.ToArray());
    }

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
}
