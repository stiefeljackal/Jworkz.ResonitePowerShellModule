using System.Management.Automation;
using System.Security.Cryptography;
using MimeDetective;

using PathUtil = System.IO.Path;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Commands.Assets;

using Core.Utilities;
using Core.Models.Abstract;
using Commands.Abstract;
using PipeBinds;

/// <summary>
/// Retrieves an asset blob from Resonite
/// </summary>
[Cmdlet(VerbsCommon.Get, "ResoniteAssetGather", DefaultParameterSetName = PARAM_SET_SAVEFILE)]
public sealed class GetAssetGather : ResoniteAssetInfoCmdlet
{
    private const string PARAM_SET_SAVEFILE = "Save asset to local path";
    private const string PARAM_SET_SAVEFILEPIPEBIND = "Save asset from pipebind to local path";
    private const string PARAM_SET_ASMEMORYSTREAM = "Return asset as MemoryStream";

    /// <summary>
    /// Hash of the asset
    /// </summary>
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = PARAM_SET_SAVEFILE)]
    [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = PARAM_SET_SAVEFILEPIPEBIND)]
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = PARAM_SET_ASMEMORYSTREAM)]
    [ValidateNotNull]
    public override required AssetInfoPipeBind AssetInfo { get; set; }

    /// <summary>
    /// Directory path to save the downloaded file to
    /// </summary>
    [Parameter(ParameterSetName = PARAM_SET_SAVEFILE)]
    [Parameter(ParameterSetName = PARAM_SET_SAVEFILEPIPEBIND)]
    [Alias("DirectoryPath")]
    public string DirPath = string.Empty;

    /// <summary>
    /// Optional file name to save the asset as; defaults to the hash id if one is not provided
    /// </summary>
    [Parameter(ParameterSetName = PARAM_SET_SAVEFILE)]
    [Parameter(ParameterSetName = PARAM_SET_SAVEFILEPIPEBIND)]
    public string? Filename;

    /// <summary>
    /// Switch that determines if the file should be saved with its extension type
    /// </summary>
    [Parameter(ParameterSetName = PARAM_SET_SAVEFILE)]
    [Parameter(ParameterSetName = PARAM_SET_SAVEFILEPIPEBIND)]
    public SwitchParameter IncludeExtension;

    /// <summary>
    /// Switch that determines if the directory should be created if it does not exist.
    /// </summary>
    [Parameter(ParameterSetName = PARAM_SET_SAVEFILE)]
    [Parameter(ParameterSetName = PARAM_SET_SAVEFILEPIPEBIND)]
    public SwitchParameter CreateDirectory;

    /// <summary>
    /// Switch that determines if the stream should be returned instead
    /// </summary>
    [Parameter(Mandatory = true, ParameterSetName = PARAM_SET_ASMEMORYSTREAM)]
    public SwitchParameter AsMemoryStream;

    public GetAssetGather() : base() { }

    public GetAssetGather(IFileSystem fileSystem) : base(fileSystem) { }

    protected override void PrepareCmdlet()
    {
        base.PrepareCmdlet();
        if (string.IsNullOrEmpty(DirPath))
        {
            DirPath = ".";
        }
    }

    protected override void ExecuteCmdlet()
    {
        var hashId = HashId;
        var stream = Client!.GatherAsset(hashId).GetAwaiterResult();


        if (AsMemoryStream)
        {
            MemoryStream memoryStream = new();
            stream.CopyTo(memoryStream);
            stream.Dispose();
            memoryStream.Position = 0;
            WriteObject(memoryStream);
            return;
        }

        var filename = Filename ?? hashId;
        FileType? fileType = null;

        if (CreateDirectory && !FileSystem.DirectoryExists(DirPath))
        {
            FileSystem.CreateDirectory(DirPath);
        }

        var fullPath = PathUtil.Combine(DirPath, filename);
        using (var fileStream = FileSystem.CreateFileStream(fullPath, FileMode.Create))
        {
            stream.CopyToAsync(fileStream).GetAwaiterResult();
            stream.Dispose();
            using (var sha = SHA256.Create())
            {
                fileStream.Position = 0;
                if (sha.ComputeHash(fileStream).ToHex() != hashId)
                {
                    WriteWarning("The chucksum of the downloaded file does not match its hash id");
                }
            }
            if (IncludeExtension)
            {
                fileStream.Position = 0;
                fileType = FileSystem.GetFileType(fileStream);
            }
        }

        if (fileType == null)
        {
            return;
        }

        var extension = fileType.GetExtension();

        if (extension == string.Empty)
        {
            WriteVerbose($"Unable to find extension for '{filename}'");
            return;
        }

        filename += $".{extension}";
        FileSystem.RenameFile(fullPath, PathUtil.Combine(DirPath, filename), true);
    }
}
