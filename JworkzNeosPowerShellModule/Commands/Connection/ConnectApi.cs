using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using CloudX.Shared;
using System.Net.NetworkInformation;
using JworkzNeosPowerShellModule.Utilities;

namespace JworkzNeosPowerShellModule.Commands.Connection;

using Abstract;
using JworkzNeosPowerShellModule.Clients;
using JworkzNeosPowerShellModule.Clients.Abstract;

/// <summary>
/// Connects to the NeosVR Api interface via CloudX with either the default or provided Uris
/// </summary>
[Cmdlet("Connect", "NeosApi", DefaultParameterSetName = PARAM_SET_CREDENTIALONLY)]
public class ConnectApi : BasePSCmdlet
{
    private const string PARAM_SET_CREDENTIALONLY = "Credential";
    private const string PARAM_SET_CUSTOMCLIENT = "Custom Client";

    /// <summary>
    /// NeosVR credential to use with the CloudX client
    /// </summary>
    [Parameter(Mandatory = true, ParameterSetName = PARAM_SET_CREDENTIALONLY, Position = 0)]
    [Parameter(Mandatory = true, ParameterSetName = PARAM_SET_CUSTOMCLIENT, Position = 0)]
    public PSCredential? Credential;

    /// <summary>
    /// Api Uri that the client needs to communicate to for general information
    /// </summary>
    [Parameter(Mandatory = true, ParameterSetName = PARAM_SET_CUSTOMCLIENT)]
    public string ApiUri = "";

    /// <summary>
    /// Asset Uri that the client needs to communicate to for assetes
    /// </summary>
    public string AssetUri = "";

    /// <summary>
    /// Switch that determines if the client should be returned instead of being set as the current
    /// </summary>
    [Parameter(Mandatory = false, ParameterSetName = PARAM_SET_CREDENTIALONLY)]
    [Parameter(Mandatory = false, ParameterSetName = PARAM_SET_CUSTOMCLIENT)]
    public SwitchParameter ReturnClient;

    protected override void ProcessRecord()
    {
        if (Credential == null)
        {
            this.WriteCredentialNull();
            return;
        }
        try
        {
            var client = Connect().GetAwaiter().GetResult();

            if (ReturnClient.ToBool())
            {
                WriteObject(client);
            }
            else
            {
                CloudXInterfacePool.Current = client;
            }
        }
        catch (Exception ex)
        {
            ex.Data["TimeStampUtc"] = DateTime.UtcNow;
            throw;
        }
    }

    protected async Task<ICloudInterfaceClient> Connect()
    {
        ICloudInterfaceClient? client = null;

        if (IsParamSpecified("ApiUri"))
        {
            if (!IsHostPingable(ApiUri))
            {
                throw new PSArgumentException($"Host '{ApiUri}' is not reachable");
            }

            throw new NotImplementedException("Cannot provided a different Uri as of yet");
        }
        else
        {
            client = new CloudXInterfaceClient();
        }

        await client.Login(Credential!);

        return client;
    }

    private static bool IsHostPingable(string uri)
    {
        var isPingable = false;
        Ping? pinger = null;

        try
        {
            pinger = new Ping();
            var reply = pinger.Send(uri);
            isPingable = reply.Status == IPStatus.Success;
        }
        catch (PingException) { }
        finally
        {
            pinger?.Dispose();
        }

        return isPingable;
    }
}
