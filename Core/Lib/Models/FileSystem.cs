using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jworkz.ResonitePowerShellModule.Core.Models;

using Core.Models.Abstract;
using System.IO;

[ExcludeFromCodeCoverage]
internal class FileSystem : IFileSystem
{
    public Stream OpenRead(string path) => File.OpenRead(path);
}
