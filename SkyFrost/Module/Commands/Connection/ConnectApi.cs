using System.Management.Automation;
using System.Net.NetworkInformation;
using SkyFrost.Base;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Commands.Connection;

using Core.Commands.Abstract;
using Core.Utilities;
using Clients;
using Clients.Abstract;
using Elements.Core;

/// <summary>
/// Connects to the Resonite Api interface via SkyFrost with either the default or provided Uris
/// </summary>
[Cmdlet("Connect", "ResoniteApi", DefaultParameterSetName = PARAM_SET_CREDENTIALONLY)]
[OutputType([typeof(ISkyFrostInterfaceClient), typeof(void)])]
public class ConnectApi : BasePSCmdlet
{
    private const string PARAM_SET_CREDENTIALONLY = "Credential";

    private static readonly string _productName;

    private static readonly string _productVersion;

    /// <summary>
    /// Resonite credential to use with the SkyfrostInterface client
    /// </summary>
    [Parameter(Mandatory = true, ParameterSetName = PARAM_SET_CREDENTIALONLY, Position = 0)]
    public PSCredential? Credential;

    /// <summary>
    /// 
    /// </summary>
    [Parameter(Mandatory = false, ParameterSetName = PARAM_SET_CREDENTIALONLY)]
    public SkyFrostConfig? Config = SkyFrostConfig.SKYFROST_PRODUCTION;

    /// <summary>
    /// Switch that determines if the client should be returned instead of being set as the current
    /// </summary>
    [Parameter(Mandatory = false, ParameterSetName = PARAM_SET_CREDENTIALONLY)]
    public SwitchParameter ReturnClient;

    /// <summary>
    /// Switch that determines if SignalR should be disabled
    /// </summary>
    [Parameter(Mandatory = false, ParameterSetName = PARAM_SET_CREDENTIALONLY)]
    public SwitchParameter DisableSignalR;

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

        var noopDelegate = (string msg) => { };
        UniLog.OnError += noopDelegate;
        UniLog.OnWarning += noopDelegate;
        UniLog.OnLog += noopDelegate;
    }

    protected override void ExecuteCmdlet()
    {
        if (Credential == null)
        {
            this.WriteCredentialNull();
            return;
        }

        var client = Connect().GetAwaiter().GetResult();

        if (ReturnClient.ToBool())
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

        client = new SkyFrostInterfaceClient(Config, _productName, _productVersion, DisableSignalR.ToBool());
        var userStatusSrc = new PowerShellStatusSource(client, _productName, _productVersion);
        client.StatusSource = userStatusSrc;

        await client.Login(Credential!);

        return client;
    }
}
