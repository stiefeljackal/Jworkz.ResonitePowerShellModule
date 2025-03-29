using System.Diagnostics.CodeAnalysis;
using SkyFrost.Base;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Test.PipeBinds;

using SkyFrost.Clients.Abstract;
using SkyFrost.PipeBinds;

[ExcludeFromCodeCoverage]
public class RecordIdPipeBindUnitTest
{
    [Fact]
    public void CallGetRecordId_ValidRecordId_ReturnsRecordId()
    {
        Mock<ISkyFrostInterfaceClient> skyFrostClientMock = new();

        RecordId expectedRecordId = new(GlobalConstants.MOCK_USER_ID, GlobalConstants.MOCK_RECORD_ID);
        RecordIdPipeBind recordPipeBind = new(expectedRecordId);

        var recordId = recordPipeBind.GetRecordId(skyFrostClientMock.Object);
        Assert.NotNull(recordId);
        Assert.Equal(expectedRecordId, recordId);
    }

    [Fact]
    public void CallGetRecordId_ValidRecordUri_ReturnsRecordId()
    {
        var profile = PlatformProfile.RESONITE;
        Uri uriMock = new($"{profile.RecordScheme}:///{GlobalConstants.MOCK_USER_ID}/{GlobalConstants.MOCK_RECORD_ID}");
        Mock<ISkyFrostInterfaceClient> skyFrostClientMock = new();
        skyFrostClientMock
            .Setup(m => m.PlatformProfile)
            .Returns(() => profile);


        RecordIdPipeBind recordPipeBind = new(uriMock);

        var recordId = recordPipeBind.GetRecordId(skyFrostClientMock.Object);
        Assert.NotNull(recordId);
        Assert.Equal(new RecordId(GlobalConstants.MOCK_USER_ID, GlobalConstants.MOCK_RECORD_ID), recordId);
    }
}
