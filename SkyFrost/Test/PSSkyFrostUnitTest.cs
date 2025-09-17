using Jworkz.ResonitePowerShellModule.Core.Mocks.Abstract;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Test;

[ExcludeFromCodeCoverage]
public class PSSkyFrostUnitTest : IPSResoUnitTestScope<PSSkyFrostUnitTestScope>
{
    private static PSSkyFrostUnitTestScope? _testScope;

    public PSSkyFrostUnitTestScope TestScope => GetTestScope();

    public static PSSkyFrostUnitTestScope GetTestScope() =>
        _testScope ??= new PSSkyFrostUnitTestScope();
}
