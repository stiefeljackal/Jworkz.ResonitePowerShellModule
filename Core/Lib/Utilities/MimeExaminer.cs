using MimeDetective;

namespace Jworkz.ResonitePowerShellModule.Core.Utilities;

public static class MimeExaminer
{

    public static FileType Inspect(Stream stream) => stream.GetFileType();

    public static FileType Inspect(byte[] bytes) => MimeTypes.GetFileType(bytes);
}
