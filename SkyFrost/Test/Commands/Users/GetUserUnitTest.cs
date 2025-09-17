using SkyFrost.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation.Runspaces;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Test.Commands.Users;

using SkyFrost.Clients.Abstract;
using SkyFrost.Commands.Users;
using System.Management.Automation;

[ExcludeFromCodeCoverage]
public sealed class GetUserUnitTest : PSSkyFrostUnitTest
{
    private static readonly string[] Params = ["Identity", "UserId", "Name"];

    private static readonly string[] ValidIdentities = [GlobalConstants.MOCK_USER_ID, GlobalConstants.MOCK_USER_USERNAME];

    [Theory]
    [MemberData(nameof(ValidPSArgs))]
    public void ExecuteCmdlet_ValidUserIdentity_ReturnsUser(PSCommand psCommand, Mock<ISkyFrostInterfaceClient> sfClientMock, string userIdentity)
    {
        var results = GetTestScope().ExecuteCmdlet(psCommand);

        sfClientMock.Verify(m => m.GetUser(It.Is<string>(arg => arg == userIdentity)), Times.Once);
        Assert.Same(GlobalConstants.MOCK_USER, results.First().BaseObject);
        Assert.Single(results);
    }

    private static Mock<ISkyFrostInterfaceClient> SetupMockClient(string userIdentity)
    {
        Mock<ISkyFrostInterfaceClient> sfClientMock = new();
        sfClientMock
            .Setup(m => m.GetUser(It.Is<string>(arg => arg == userIdentity)))
            .Returns(() => Task.FromResult(GlobalConstants.MOCK_USER));

        return sfClientMock;
    }

    public static IEnumerable<object[]> ValidIdentitiesArgs => ValidIdentities.Select<string, string[]>(id => [ id ]);

    public static IEnumerable<object[]> ValidPSArgs
    {
        get
        {
            foreach (var paramName in Params)
            {
                foreach (var userIdentity in ValidIdentities)
                {
                    var sfClientMock = SetupMockClient(userIdentity);
                    var psCommand = new PSCommand()
                        .AddCommand("Get-SfUser", sfClientMock, new CommandParameter(paramName, userIdentity));

                    yield return [psCommand, sfClientMock, userIdentity];
                }
            }

            foreach (var userIdentity in ValidIdentities)
            {
                var sfClientMock = SetupMockClient(userIdentity);
                var psCommand = new PSCommand()
                    .AddEchoCommand(userIdentity)
                    .AddCommand("Get-SfUser", sfClientMock);

                yield return [psCommand, sfClientMock, userIdentity];
            }

            foreach (var userIdentity in ValidIdentities)
            {
                var sfClientMock = SetupMockClient(userIdentity);
                var psCommand = new PSCommand()
                    .AddCommand("Get-SfUser", sfClientMock)
                    .AddArgument(userIdentity);

                yield return [psCommand, sfClientMock, userIdentity];
            }
        }
    }
}
