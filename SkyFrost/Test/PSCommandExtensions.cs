using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Test;

using Core.Mocks;
using SkyFrost.Clients.Abstract;

[ExcludeFromCodeCoverage]
public static class PSCommandExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PSCommand AddCommand(this PSCommand psCommand, string cmdletString, Mock<ISkyFrostInterfaceClient> sfClientMock) =>
        AddCommand(psCommand, cmdletString, sfClientMock, null);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PSCommand AddCommand(this PSCommand psCommand, string cmdletString, Mock<ISkyFrostInterfaceClient> sfClientMock, params CommandParameter[]? parameters) =>
        psCommand.AddCommand(cmdletString, [.. parameters ?? [], new CommandParameter("Client", sfClientMock.Object)]);
}
