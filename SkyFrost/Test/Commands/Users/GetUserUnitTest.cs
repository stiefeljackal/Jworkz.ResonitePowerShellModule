using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using SkyFrost.Base;
using Jworkz.ResonitePowerShellModule.SkyFrost.Test.Fixtures;

[assembly: AssemblyFixture(typeof(SkyFrostPSFixture))]

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Test.Commands.Users;

using SkyFrost.Clients.Abstract;

[ExcludeFromCodeCoverage]
public sealed class GetUserUnitTest(SkyFrostPSFixture sfPsFixture)
{
    private static readonly string[] SingleUserParams = ["Identity", "UserId", "Name"];

    private static readonly string[] ValidIdentities = [GlobalConstants.MOCK_USER_ID, GlobalConstants.MOCK_USER_USERNAME];

    private static readonly string?[] InvalidIdentities = [string.Empty, null, "   "];

    private static object[][]? _validPsArgs;

    private static object[][]? _invalidPsArgs;

    public static object[][] ValidPSArgs => _validPsArgs ??= [.. GenerateValidSingleUserPSArgs()];

    public static object[][] InvalidPSArgs => _invalidPsArgs ??= [.. GenerateInvalidSingleUserPSArgs()];

    [Theory]
    [MemberData(nameof(ValidPSArgs))]
    public async Task ExecuteCmdlet_ValidUserIdentity_ReturnsUser(PSCommand psCommand, Mock<ISkyFrostInterfaceClient> sfClientMock, string userIdentity)
    {
        var results = await sfPsFixture.ExecuteCmdletTaskAsync(psCommand);

        sfClientMock.Verify(m => m.GetUser(It.Is<string>(arg => arg == userIdentity)), Times.Once);
        var userResult = results.First().BaseObject;
        Assert.Same(GlobalConstants.MOCK_USER, userResult);
        Assert.IsType<User>(userResult);
        Assert.Single(results);
    }

    [Theory]
    [MemberData(nameof(InvalidPSArgs))]
    public async Task ExecuteCmdlet_InvalidUserIdentity_ThrowsError(PSCommand psCommand)
    {
        await PSAssert.ThrowsParameterBindingValidationException(
            async () => await sfPsFixture.ExecuteCmdletTaskAsync(psCommand)
        );
    }

    [Fact]
    public async Task ExecuteCmdlet_ValidQuery_ReturnsListOfUsers()
    {
        var sfClientMock = SetupMockClient();

        var results = await sfPsFixture
            .ExecuteCmdletTaskAsync("Get-SfUser", sfClientMock, new CommandParameter("Query", GlobalConstants.MOCK_USER_USERNAME));

        sfClientMock.Verify(m => m.GetUsers(It.Is<string>(arg => arg == GlobalConstants.MOCK_USER_USERNAME)), Times.Once);
        var userListResult = results.First().BaseObject;
        Assert.Same(GlobalConstants.MOCK_USER_LIST, userListResult);
        Assert.IsType<IEnumerable<User>>(userListResult, false);
        Assert.Single(results);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("     ")]
    public async Task ExecuteCmdlet_InvalidQuery_ThrowsError(string? invalidQuery)
    {
        var sfClientMock = SetupMockClient();

        var psCommand = new PSCommand()
            .AddCommand("Get-SfUser", sfClientMock, new CommandParameter("Query", invalidQuery));

        await PSAssert.ThrowsParameterBindingValidationException(
            async () => await sfPsFixture.ExecuteCmdletTaskAsync(psCommand)
        );
    }

    private static Mock<ISkyFrostInterfaceClient> SetupMockClient()
    {
        Mock<ISkyFrostInterfaceClient> sfClientMock = new();
        sfClientMock
            .Setup(m => m.GetUser(It.IsAny<string>()))
            .Returns(() => Task.FromResult(GlobalConstants.MOCK_USER));
        sfClientMock
            .Setup(m => m.GetUsers(It.IsAny<string>()))
            .Returns(() => Task.FromResult(GlobalConstants.MOCK_USER_LIST.AsEnumerable()));

        return sfClientMock;
    }

    private static IEnumerable<object[]> GenerateValidSingleUserPSArgs()
    {
        foreach (var paramName in SingleUserParams)
        {
            foreach (var userIdentity in ValidIdentities)
            {
                var sfClientMock = SetupMockClient();
                var psCommand = new PSCommand()
                    .AddCommand("Get-SfUser", sfClientMock, new CommandParameter(paramName, userIdentity));

                yield return [psCommand, sfClientMock, userIdentity];
            }
        }

        foreach (var userIdentity in ValidIdentities)
        {
            var sfClientMock = SetupMockClient();
            var psCommand = new PSCommand()
                .AddEchoCommand(userIdentity)
                .AddCommand("Get-SfUser", sfClientMock);

            yield return [psCommand, sfClientMock, userIdentity];
        }

        foreach (var userIdentity in ValidIdentities)
        {
            var sfClientMock = SetupMockClient();
            var psCommand = new PSCommand()
                .AddCommand("Get-SfUser", sfClientMock)
                .AddArgument(userIdentity);

            yield return [psCommand, sfClientMock, userIdentity];
        }
    }

    private static IEnumerable<object[]> GenerateInvalidSingleUserPSArgs()
    {
        var sfClientMock = SetupMockClient();
        foreach (var paramName in SingleUserParams)
        {
            foreach (var invalidIdentity in InvalidIdentities)
            {
                var psCommand = new PSCommand()
                    .AddCommand("Get-SfUser", sfClientMock, new CommandParameter(paramName, invalidIdentity));

                yield return [psCommand];
            }
        }

        foreach (var invalidIdentity in InvalidIdentities)
        {
            var psCommand = new PSCommand()
                .AddEchoCommand(invalidIdentity)
                .AddCommand("Get-SfUser", sfClientMock);

            yield return [psCommand];
        }

        foreach (var invalidIdentity in InvalidIdentities)
        {
            var psCommand = new PSCommand()
                .AddCommand("Get-SfUser", sfClientMock)
                .AddArgument(invalidIdentity);

            yield return [psCommand];
        }
    }
}
