using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Test.Fixtures;

using SkyFrost.Clients.Abstract;

[ExcludeFromCodeCoverage]
public class SkyFrostPSFixture : PSFixture
{
    public SkyFrostPSFixture() : base("Jworkz.ResonitePowerShellModule.SkyFrost.dll")
    {
    }

    public async Task<IEnumerable<PSObject>> ExecuteCmdletTaskAsync(string cmdletString, Mock<ISkyFrostInterfaceClient> sfClientMock)
    {
        var result = await ExecuteCmdletTaskAsync(cmdletString, sfClientMock, null);
        return result;
    }

    public async Task<IEnumerable<PSObject>> ExecuteCmdletTaskAsync(string cmdletString, Mock<ISkyFrostInterfaceClient> sfClientMock, params CommandParameter[]? parameters)
    {
        CommandParameter[] modifiedParameters = [
            .. parameters ?? [],
            new CommandParameter("Client", sfClientMock.Object)
        ];

        var result = await ExecuteCmdletTaskAsync(cmdletString, modifiedParameters);
        return result;
    }
}