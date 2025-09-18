using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SkyFrost.Base;

namespace Jworkz.ResonitePowerShellModule.SkyFrost.Commands.Users;

using Core.Utilities;
using SkyFrost.Commands.Abstract;

[Cmdlet(VerbsCommon.Get, "SfUser", DefaultParameterSetName = PARAM_SET_GETUSERBYPOSITION)]
[OutputType(typeof(User), typeof(IEnumerable<User>))]
public sealed class GetUser : SkyFrostConnectedCmdlet
{
    private const string PARAM_SET_GETUSERBYVALUEPIPELINE = "Get user by value pipeline";
    private const string PARAM_SET_GETUSERBYPOSITION = "Get user by position";
    private const string PARAM_SET_GETUSERSBYQUERY = "Get users by query";

    [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = PARAM_SET_GETUSERBYVALUEPIPELINE)]
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = PARAM_SET_GETUSERBYPOSITION)]
    [Alias("UserId", "Name")]
    [ValidateNotNullOrWhiteSpace]
    public string? Identity { get; set; }

    [Parameter(Mandatory = true, ParameterSetName = PARAM_SET_GETUSERSBYQUERY)]
    [ValidateNotNullOrWhiteSpace]
    public string? Query { get; set; }

    protected override void ExecuteCmdlet()
    {
        if (!string.IsNullOrWhiteSpace(Identity))
        {
            var user = Client!.GetUser(Identity).GetAwaiterResult();
            WriteObject(user);
        }
        else if (!string.IsNullOrWhiteSpace(Query))
        {
            var users = Client!.GetUsers(Query).GetAwaiterResult();
            WriteObject(users);
        }
    }
}
