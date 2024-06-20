using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jworkz.ResonitePowerShellModule.Core.Models.Abstract;

public interface IPSState
{
    string? GetCurrentPwd();
}
