using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace JworkzNeosPowerShellModule.Secrets.Windows.Models;

public class CriticalCredentialHandle : CriticalHandleZeroOrMinusOneIsInvalid
{
    public CriticalCredentialHandle(IntPtr preexistingHandle)
    {
        SetHandle(preexistingHandle);
    }

    public WindowsVaultCredential? GetCredential()
    {
        if (!IsInvalid)
        {
            NativeVaultCredential? ncred = (NativeVaultCredential?)Marshal.PtrToStructure(handle, typeof(NativeVaultCredential));
            if (!ncred.HasValue) { return null;  }

            return (WindowsVaultCredential)ncred;
        }
        else
        {
            throw new InvalidOperationException("Invalid CriticalHandle!");
        }
    }

    override protected bool ReleaseHandle()
    {
        if (!IsInvalid)
        {
            CredFree(handle);
            SetHandleAsInvalid();
            return true;
        }
        return false;
    }

    [DllImport("Advapi32.dll", EntryPoint = "CredFree", SetLastError = true)]
    private static extern bool CredFree([In] IntPtr cred);
}
