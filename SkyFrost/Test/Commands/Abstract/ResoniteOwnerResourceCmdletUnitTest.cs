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
    [MemberData(nameof(OwnerData))]
    public void ExecuteCmd_ValidOwner_HasValidOwnerData(OwnerPipeBind ownerMock, OwnerType expectedOwnerType)
    {
        var cmdlet = SetupCmdlet(ownerMock);

        cmdlet.StartProcessExecution();

        Assert.Equal(ownerMock, cmdlet.Owner);
        Assert.Equal(expectedOwnerType, cmdlet.OwnerType);
        Assert.Equal(ownerMock.OwnerId, cmdlet.OwnerId);
    }

    [Theory]
    [MemberData(nameof(InvalidOwnerWithExpectedOutcomeData))]
    public void ExecuteCmd_InvalidOwner_HadEmptyOrInvalidData(OwnerPipeBind? ownerMock, OwnerType expectedOwnerType)
    {
        var cmdlet = SetupCmdlet(ownerMock, false);

        cmdlet.StartProcessExecution();

        Assert.Equal(ownerMock, cmdlet.Owner);
        Assert.Equal(expectedOwnerType, cmdlet.OwnerType);
        Assert.Equal(ownerMock?.OwnerId ?? string.Empty, cmdlet.Owner != null ? cmdlet.OwnerId : string.Empty);
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

    /// <summary>
    /// Theory data used to test for valid owners.
    /// </summary>
    public static IEnumerable<object[]> OwnerData =>
    new object[][]
    {
            [new User { Id = GlobalConstants.USER_OWNER_ID }, OwnerType.User],
            [new Group { GroupId = GlobalConstants.GROUP_OWNER_ID }, OwnerType.Group],
            [GlobalConstants.USER_OWNER_ID, OwnerType.User],
            [GlobalConstants.GROUP_OWNER_ID, OwnerType.Group],
    };

    /// <summary>
    /// Theory data used to test for invalid owners.
    /// </summary>
    public static IEnumerable<object?[]> InvalidOwnerWithExpectedOutcomeData =>
        new object?[][]
        {
            [new OwnerPipeBind(string.Empty), OwnerType.INVALID],
            [new User { Id = GlobalConstants.MACHINE_ID }, OwnerType.Machine],
            [(OwnerPipeBind?)null, OwnerType.INVALID],
            [GlobalConstants.MACHINE_ID, OwnerType.Machine]
        };

    public static IEnumerable<object?[]> InvalidOwnerData =>
        new object?[][]
        {
            [new OwnerPipeBind(string.Empty)],
            [new User { Id = GlobalConstants.MACHINE_ID }],
            [(OwnerPipeBind?)null],
            [GlobalConstants.MACHINE_ID]
        };
}
