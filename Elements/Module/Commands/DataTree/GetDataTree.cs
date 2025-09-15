using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Elements.Core;

namespace Jworkz.ResonitePowerShellModule.Elements.Commands.DataTree;

using Core.Commands.Abstract;
using Models.DataTree;

[Cmdlet("Get", "ResDataTree", DefaultParameterSetName = PARAM_SET_LOADFILEPATH)]
public class GetDataTree : BasePSCmdlet
{
    private const string PARAM_SET_LOADFILEPATH = "Load from File Path";
    private const string PARAM_SET_LOADFILEPATHASFORMAT = "Load from File Path as Format";
    private const string PARAM_SET_LOADSTREAM = "Load from Stream";

    [Parameter(Mandatory = true, ParameterSetName = PARAM_SET_LOADFILEPATH, ValueFromPipeline = true, Position = 0)]
    [Parameter(Mandatory = true, ParameterSetName = PARAM_SET_LOADFILEPATHASFORMAT, ValueFromPipeline = true, Position = 0)]
    [Alias("Path")]
    public string? FilePath;

    [Parameter(Mandatory = true, ParameterSetName = PARAM_SET_LOADFILEPATHASFORMAT, Position = 1)]
    public string? Format;

    [Parameter(Mandatory = true, ParameterSetName = PARAM_SET_LOADSTREAM, ValueFromPipeline = true, Position = 0)]
    public Stream? Stream;

    protected override void PrepareCmdlet()
    {
        if (Stream != null || string.IsNullOrEmpty(FilePath) || !string.IsNullOrEmpty(Format) || DataTreeConverter.IsSupportedFormat(FilePath)) { return; }

        var ext = Path.GetExtension(FilePath);
        throw new PSArgumentException($"Extension '{ext}' is not a supported extension.");
    }

    protected override void ExecuteCmdlet()
    {
        DataTreeDictionary dtDictionary;

        switch(ParameterSetName)
        {
            case PARAM_SET_LOADSTREAM:
                dtDictionary = DataTreeConverter.LoadAuto(Stream);
                break;
            default:
                dtDictionary = DataTreeConverter.Load(FilePath, Format);
                break;
        }

        WriteObject(new SimpleDataTreeRootNode(dtDictionary));
    }
}
