using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Elements.Core;

using DataTreeCompression = Elements.Core.DataTreeConverter.Compression;

namespace Jworkz.ResonitePowerShellModule.Elements.Commands.DataTree;

using Core.Commands.Abstract;
using Jworkz.ResonitePowerShellModule.Elements.Models.DataTree;

[Cmdlet("Set", "ResoniteDataTree", DefaultParameterSetName = PARAM_SET_SAVETOFILE)]
public class SetDataTree : BasePSCmdlet
{
    private const string PARAM_SET_SAVETOFILE = "Save to File";

    [Parameter(Mandatory = true, ParameterSetName = PARAM_SET_SAVETOFILE, ValueFromPipeline = true)]
    public SimpleDataTreeRootNode? DataTree;

    [Parameter(Mandatory = true, ParameterSetName = PARAM_SET_SAVETOFILE, Position = 1)]
    [Alias("Path")]
    public string? FilePath;

    [Parameter(Mandatory = false, ParameterSetName = PARAM_SET_SAVETOFILE, Position = 2)]
    public DataTreeCompression Compression;

    protected override void PrepareCmdlet()
    {
        if (Compression == DataTreeCompression.None)
        {
            var ext = Path.GetExtension(FilePath);

            switch (ext)
            {
                case ".brson":
                    Compression = DataTreeCompression.Brotli;
                    break;
                case ".7zbson":
                    Compression = DataTreeCompression.LZMA;
                    break;
                case ".lz4bson":
                    Compression = DataTreeCompression.LZ4;
                    break;
                default:
                    throw new NotSupportedException($"Extension '{ext}' does not have an associated compression type.");

            }
        }
    }

    protected override void ExecuteCmdlet()
    {
        DataTreeConverter.Save(DataTree!.GetDataTreeDictionary(), FilePath, Compression);
    }
}
