using System.Runtime.InteropServices;
using System.Security;
using FileTime = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace JworkzNeosPowerShellModule.Secrets.Windows.Models;

using Enums;
using JworkzNeosPowerShellModule.Utilities;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public struct WindowsVaultCredential
{
    public uint Flags;
    public CredType Type;
    public string TargetName;
    public string Comment;
    public FileTime LastWritten;
    public uint CredentialBlobSize;
    public SecureString CredentialBlob;
    public uint Persist;
    public uint AttributeCount;
    public IntPtr Attributes;
    public string TargetAlias;
    public string UserName;
    
    public static explicit operator WindowsVaultCredential(NativeVaultCredential ncred)
    {
        return new WindowsVaultCredential
        {
            CredentialBlobSize = ncred.CredentialBlobSize,
            CredentialBlob = Marshal.PtrToStringUni(ncred.CredentialBlob, (int)ncred.CredentialBlobSize / 2).ToSecureString(),
            UserName = Marshal.PtrToStringUni(ncred.UserName) ?? string.Empty,
            TargetName = Marshal.PtrToStringUni(ncred.TargetName) ?? string.Empty,
            TargetAlias = Marshal.PtrToStringUni(ncred.TargetAlias) ?? string.Empty,
            Type = ncred.Type,
            Flags = ncred.Flags,
            Persist = ncred.Persist
        }; ;
    }
}
