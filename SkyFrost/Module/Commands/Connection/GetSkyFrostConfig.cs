using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Management.Automation;
using SkyFrost.Base;
using IOPath = System.IO.Path;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Commands.Connection;

using Core.Commands.Abstract;
using Core.Utilities;

/// <summary>
/// Loads a SkyFrost configration that can be used to setup a SkyFrost client
/// </summary>
[Cmdlet(VerbsCommon.Get, "ResoniteSkyFrostConfig")]
[OutputType(typeof(SkyFrostConfig))]
public class GetSkyFrostConfig : BasePSCmdlet
{
    [Parameter(Mandatory = true, Position = 0)]
    [ValidateNotNullOrEmpty]
    public string? Path;

    protected override void ExecuteCmdlet()
    {
        Path.ThrowOnNullOrEmpty($"Parameter '{nameof(Path)}' cannot be null or empty.");

        var filePath = IOPath.GetFullPath(IOPath.Combine(CurrentLocation, Path!));
        using var filestream = FileSystem.OpenRead(filePath);
        var skyFrostConfig = JsonSerializer.Deserialize<SkyFrostConfig>(filestream);

        WriteObject(skyFrostConfig);
    }
}
