using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Test;

using SkyFrost.Clients.Abstract;

[ExcludeFromCodeCoverage]
public class PSSkyFrostUnitTestScope : PSResoUnitTestScope
{
    protected override bool IsExecutionPolicyUpdateRequired => false;

    public PSSkyFrostUnitTestScope() : base("Jworkz.ResonitePowerShellModule.SkyFrost.dll")
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<PSObject> ExecuteCmdlet(string cmdletString, Mock<ISkyFrostInterfaceClient> sfClientMock) =>
        ExecuteCmdlet(cmdletString, sfClientMock, null);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<PSObject> ExecuteCmdlet(string cmdletString, Mock<ISkyFrostInterfaceClient> sfClientMock, params CommandParameter[]? parameters) =>
        ExecuteCmdlet(cmdletString, [.. parameters ?? [], new CommandParameter("Client", sfClientMock.Object)]);
}