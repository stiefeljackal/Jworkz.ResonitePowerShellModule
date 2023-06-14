using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JworkzNeosPowerShellModule.Utilities
{
    internal static class TaskExtensions
    {
        public static T GetAwaiterResult<T>(this Task<T> task)
        {
            return task.GetAwaiter().GetResult();
        }
    }
}
