using JworkzNeosPowerShellModule.Secrets.Windows.Enums;
using JworkzNeosPowerShellModule.Utilities;
using System.Runtime.InteropServices;
using FileTime = System.Runtime.InteropServices.ComTypes.FILETIME;


namespace JworkzNeosPowerShellModule.Secrets.Windows.Models;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public struct NativeVaultCredential
{
    public uint Flags;
    public CredType Type;
    public IntPtr TargetName;
    public IntPtr Comment;
    public FileTime LastWritten;
    public uint CredentialBlobSize;
    public IntPtr CredentialBlob;
    public uint Persist;
    public uint AttributeCount;
    public IntPtr Attributes;
    public IntPtr TargetAlias;
    public IntPtr UserName;

    public static explicit operator NativeVaultCredential(WindowsVaultCredential cred)
    {
        return new NativeVaultCredential
        {
            AttributeCount = 0,
            Attributes = IntPtr.Zero,
            Comment = IntPtr.Zero,
            TargetAlias = IntPtr.Zero,
            Type = CredType.GENERIC,
            Persist = 1,
            CredentialBlobSize = cred.CredentialBlobSize,
            TargetName = Marshal.StringToCoTaskMemUni(cred.TargetName),
            CredentialBlob = Marshal.StringToCoTaskMemUni(cred.CredentialBlob.ToPlainText()),
            UserName = Marshal.StringToCoTaskMemUni(Environment.UserName)

        };
    }
}
