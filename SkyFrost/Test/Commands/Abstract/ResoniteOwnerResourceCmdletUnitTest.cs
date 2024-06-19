using System.Management.Automation;
using Moq;
using SkyFrost.Base;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Test.Commands.Abstract;

using SkyFrost.Clients.Abstract;
using SkyFrost.Commands.Abstract;
using SkyFrost.PipeBinds;

public class ResoniteOwnerResourceCmdletUnitTest
{
    [Theory]
    [MemberData(nameof(ValidOwnerData))]
    public void ExecuteCmd_ValidOwner_HasValidOwnerData(OwnerPipeBind ownerMock, OwnerType expectedOwnerType)
    {
        var cmdlet = SetupCmdlet(ownerMock);

        try
        {
            cmdlet.StartProcessExecution();
            Assert.Equal(expectedOwnerType, cmdlet.OwnerType);
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
        }
    }

    [Theory]
    [MemberData(nameof(InvalidOwnerData))]
    public void ExecuteCmd_InvalidOwner_ThrowsError(OwnerPipeBind? ownerMock)
    {
        var cmdlet = SetupCmdlet(ownerMock);

        Assert.Throws<PSInvalidOperationException>(cmdlet.StartProcessExecution);
    }

    /// <summary>
    /// Setup the cmdlet for the unit test.
    /// </summary>
    /// <param name="ownerMock">The owner pipebind mock.</param>
    /// <param name="callBase">true if base calls should be performed; otherwise, false.</param>
    /// <returns></returns>
    private static ResoniteOwnerResourceCmdlet SetupCmdlet(OwnerPipeBind? ownerMock, bool callBase = true)
    {
        Mock<ISkyFrostInterfaceClient> skyFrostClientMock = new();
        Mock<ResoniteOwnerResourceCmdlet> cmdletMock = new();
        cmdletMock.CallBase = callBase;
        cmdletMock.SetupAllProperties();

        var cmdlet = cmdletMock.Object;

        cmdlet.CommandRuntime = new MockCommandRuntime<object>();
        cmdlet.Client = skyFrostClientMock.Object;
        cmdlet.Owner = ownerMock;

        return cmdlet;
    }

    public static IEnumerable<object[]> ValidOwnerData =>
        new object[][]
        {
            [new User { Id = GlobalConstants.MOCK_USER_ID }, OwnerType.User],
            [new Group { GroupId = GlobalConstants.MOCK_GROUP_ID }, OwnerType.Group],
            [GlobalConstants.MOCK_USER_ID, OwnerType.User],
            [GlobalConstants.MOCK_GROUP_ID, OwnerType.Group],
        };

    public static IEnumerable<object?[]> InvalidOwnerData =>
        new object?[][]
        {
            [new OwnerPipeBind(string.Empty)],
            [new User { Id = GlobalConstants.MOCK_MACHINE_ID }],
            [(OwnerPipeBind?)null],
            [GlobalConstants.MOCK_MACHINE_ID]
        };
}
