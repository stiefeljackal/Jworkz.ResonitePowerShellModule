using System.Management.Automation;
using System.Security.Cryptography;

using PathUtil = System.IO.Path;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Commands.Assets;

using Core.Utilities;
using Commands.Abstract;

/// <summary>
/// Retrieves an asset blob from Resonite
/// </summary>
[Cmdlet(VerbsCommon.Get, "ResoniteAssetGather", DefaultParameterSetName = PARAM_SET_SAVEFILE)]
public class GetAssetGather : ResoniteAssetInfoCmdlet
{
    private const string PARAM_SET_SAVEFILE = "Save to local path";
    private const string PARAM_SET_ASMEMORYSTREAM = "Return as MemoryStream";

    /// <summary>
    /// Optional path to save the downloaded file to
    /// </summary>
    [Parameter(Mandatory = true, ParameterSetName = PARAM_SET_SAVEFILE)]
    public string Path = string.Empty;

    /// <summary>
    /// Optional file name to save the asset as; defaults to the hash id if one is not provided
    /// </summary>
    [Parameter(Mandatory = false, ParameterSetName = PARAM_SET_SAVEFILE)]
    public string? Filename;

    /// <summary>
    /// Switch that determines if the file should be saved with its extension type
    /// </summary>
    [Parameter(Mandatory = false, ParameterSetName = PARAM_SET_SAVEFILE)]
    public SwitchParameter IncludeExtension;

    /// <summary>
    /// Switch that determines if the stream should be returned instead
    /// </summary>
    [Parameter(Mandatory = true, ParameterSetName = PARAM_SET_ASMEMORYSTREAM)]
    public SwitchParameter AsMemoryStream;

    protected override void ExecuteCmdlet()
    {
        var hashId = HashId;
        var stream = Client!.GatherAsset(hashId).GetAwaiter().GetResult();

        var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        stream.Dispose();

        switch (ParameterSetName)
        {
            case PARAM_SET_SAVEFILE:
                var filename = Filename ?? hashId;
                var bytes = memoryStream.ToArray();
                memoryStream.Dispose();

                if (IncludeExtension.ToBool())
                {
                    var results = MimeExaminer.Inspect(bytes);
                    var extension = results.FirstOrDefault()?.Definition.File.Extensions.FirstOrDefault();

                    if (!string.IsNullOrEmpty(extension))
                    {
                        filename += $".{extension}";
                    }
                    else
                    {
                        WriteVerbose($"Unable to find extension for '{filename}'");
                    }
                }
                var fullPath = PathUtil.Combine(Path, filename);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    fileStream.WriteAsync(bytes).ConfigureAwait(false);
                    using (var sha = SHA256.Create())
                    {
                        if (sha.ComputeHash(bytes).ToHex() != hashId)
                        {
                            WriteWarning("The chucksum of the downloaded file does not match its hash id");
                        }
                    }
                }
                break;
            default:
                WriteObject(memoryStream);
                break;
        }
    }
}
