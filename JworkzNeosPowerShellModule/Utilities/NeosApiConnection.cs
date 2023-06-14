using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using CloudX.Shared;
using JworkzNeosPowerShellModule.Clients.Abstract;

namespace JworkzNeosPowerShellModule.Utilities;

public static class NeosApiConnection
{
    /// <summary>
    /// Credentials used to connect to CloudX
    /// </summary>
    private static PSCredential? _psCredential;

    /// <summary>
    /// The current CloudX interface connection that is used for these cmdlets
    /// </summary>
    public static ICloudInterfaceClient? Current { get; internal set; }

    /// <summary>
    /// The connected user's username
    /// </summary>
    public static string ConnectedUserName => _psCredential?.UserName ?? string.Empty;
}