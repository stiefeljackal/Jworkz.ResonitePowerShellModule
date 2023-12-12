namespace Jworkz.ResonitePowerShellModule.Core.Utilities;

using Clients.Abstract;

public static class SkyFrostInterfacePool
{
    /// <summary>
    /// Current default client that is used for all cmdlets unless a specific client is provided
    /// </summary>
    public static ISkyFrostInterfaceClient? Current;
}
