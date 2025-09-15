using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;
using Elements.Core;
using SkyFrost.Base;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Commands.Connection;

using Core.Commands.Abstract;
using Core.Utilities;
using Clients;
using Clients.Abstract;

/// <summary>
/// Connects to the Resonite Api interface via SkyFrost with either the default or provided Uris
/// </summary>
[Cmdlet("Connect", "ResoniteApi", DefaultParameterSetName = PARAM_SET_CREDENTIALONLY)]
[OutputType([typeof(ISkyFrostInterfaceClient), typeof(void)])]
public class ConnectApi : BasePSCmdlet
{
    private const string PARAM_SET_CREDENTIALONLY = "Credential";
    private const string PARAM_SET_ANONONLY = "Anonymous";

    private static readonly string _productName;

    private static readonly string _productVersion;

    /// <summary>
    /// Resonite credential to use with the SkyfrostInterface client
    /// </summary>
    [Parameter(Mandatory = true, ParameterSetName = PARAM_SET_CREDENTIALONLY, Position = 0)]
    public PSCredential? Credential { get; set; }

    /// <summary>
    /// Configuration to interact with SkyFrost compatible infrastructure
    /// </summary>
    [Parameter(Mandatory = false, ParameterSetName = PARAM_SET_CREDENTIALONLY)]
    [Parameter(Mandatory = false, ParameterSetName = PARAM_SET_ANONONLY)]
    [ValidateNotNull]
    public SkyFrostConfig Config { get; set; } = SkyFrostConfig.SKYFROST_PRODUCTION;

    /// <summary>
    /// Determines if the client should login without credentials (anonymous)
    /// </summary>
    [Parameter(Mandatory = true, ParameterSetName = PARAM_SET_ANONONLY)]
    public SwitchParameter LoginAsAnonymous;

    /// <summary>
    /// Determines if the client should be returned instead of being set as the current
    /// </summary>
    [Parameter(Mandatory = false, ParameterSetName = PARAM_SET_CREDENTIALONLY)]
    [Parameter(Mandatory = false, ParameterSetName = PARAM_SET_ANONONLY)]
    public SwitchParameter ReturnClient;

    /// <summary>
    /// Determines if SignalR should be disabled
    /// </summary>
    [Parameter(Mandatory = false, ParameterSetName = PARAM_SET_CREDENTIALONLY)]
    [Parameter(Mandatory = false, ParameterSetName = PARAM_SET_ANONONLY)]
    public SwitchParameter DisableSignalR;

    /// <summary>
    /// Creates the client used for connecting to a SkyFrost compatible infrastructure
    /// </summary>
    internal Func<string, string, bool, SkyFrostConfig, ISkyFrostInterfaceClient> CreateClient;

    [ExcludeFromCodeCoverage]
    static ConnectApi()
    {
        switch (PSVersionInfo.PSEdition)
        {
            case "Core":
                _productName = "PowerShell Core";
                break;
            case "Desktop":
                _productName = "Windows PowerShell";
                break;
            default:
                _productName = PSVersionInfo.PSEdition;
                break;
        }

        _productVersion = PSVersionInfo.PSVersion.ToString();
    }
    
    public ConnectApi()
    {
        CreateClient = CreateInternalClient;
    }

    protected override void ExecuteCmdlet()
    {
        if (!LoginAsAnonymous && Credential == null)
        {
            throw new Exception("Provided PSCredential object is null");
        }

        var client = BindTaskToUniLog(Connect()).GetAwaiterResult();

        if (ReturnClient)
        {
            WriteObject(client);
        }
        else
        {
            SkyFrostInterfacePool.Current = client;
        }
    }

    protected async Task<ISkyFrostInterfaceClient> Connect()
    {
        ISkyFrostInterfaceClient? client;

        if (Config == null)
        {
            Config = SkyFrostConfig.SKYFROST_PRODUCTION;
        }

        client = CreateClient(_productName, _productVersion, DisableSignalR.ToBool(), Config);
        var userStatusSrc = new PowerShellStatusSource(client, _productName, _productVersion);
        client.StatusSource = userStatusSrc;

        if (Credential != null)
        {
            await client.Login(Credential);
        }

        return client;
    }

    [ExcludeFromCodeCoverage]
    private ISkyFrostInterfaceClient CreateInternalClient(string productName, string productVersion, bool isSignalRDisabled, SkyFrostConfig config) =>
        new SkyFrostInterfaceClient(config, productName, productVersion, isSignalRDisabled);
}
