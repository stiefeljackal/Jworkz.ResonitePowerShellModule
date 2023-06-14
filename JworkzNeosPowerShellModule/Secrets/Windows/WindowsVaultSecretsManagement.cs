using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Security;

namespace JworkzNeosPowerShellModule.Secrets.Windows;

using Abstract;
using Enums;
using Models;
using JworkzNeosPowerShellModule.Utilities;

public class WindowsVaultSecretsManagement : ISecretsManagement
{
    private const int MAX_PASSWORD_BYTE_LIMIT = 512 * 5;

    private const uint PERSIST_COUNT = 3;

    /// <summary>
    /// Writes a NeosVR credential into the Windows Vault
    /// </summary>
    /// <param name="key">Name of the key to save the credentials under</param>
    /// <param name="username">Username of the NeosVR account</param>
    /// <param name="password">Password for the NeosVR account</param>
    /// <param name="overwrite">To overwrite an existing credential under the key (Not used)</param>
    /// <returns>True if the credential was successfully added, false if the credential was not added</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="Exception"></exception>
    public bool AddCredential(string key, string username, SecureString password, bool overwrite = true)
    {
        var pwdByteArr = password.ToBytes();

        if (pwdByteArr?.Length > MAX_PASSWORD_BYTE_LIMIT) {
            throw new ArgumentOutOfRangeException("password", $"The password has exceeded {MAX_PASSWORD_BYTE_LIMIT} bytes.");
        }

        var credential = new NativeVaultCredential()
        {
            AttributeCount = 0,
            Attributes = IntPtr.Zero,
            Comment = IntPtr.Zero,
            TargetAlias = IntPtr.Zero,
            Type = CredType.GENERIC,
            Persist = PERSIST_COUNT,
            CredentialBlobSize = (uint)(pwdByteArr == null ? 0 : pwdByteArr.Length),
            TargetName = Marshal.StringToCoTaskMemUni(CreateNeosKey(key)),
            CredentialBlob = Marshal.StringToCoTaskMemUni(password.ToPlainText()),
            UserName = Marshal.StringToCoTaskMemUni(username)
        };

        var hasWritten = CredWrite(ref credential, 0);
        Marshal.FreeCoTaskMem(credential.TargetName);
        Marshal.FreeCoTaskMem(credential.CredentialBlob);
        Marshal.FreeCoTaskMem(credential.UserName);

        if (!hasWritten)
        {
            var lastErrorCode = Marshal.GetLastWin32Error();
            throw new Exception($"CredWrite failed with the error code {lastErrorCode}");

        }

        return hasWritten;
    }

    /// <summary>
    /// Returns a NeosVR credential from the Windows Vault based on a given key
    /// </summary>
    /// <param name="key">Name of the key to use for fetching a NeosVR credential</param>
    /// <returns>Credential based on the given key if any</returns>
    public PSCredential? GetCredential(string key)
    {
        var isSuccess = CredRead(CreateNeosKey(key), CredType.GENERIC, 0, out IntPtr credPtr);

        if (!isSuccess) { return null; }

        var critCredHandle = new CriticalCredentialHandle(credPtr);
        var cred = critCredHandle.GetCredential();

        return cred != null ? new PSCredential(cred?.UserName, cred?.CredentialBlob) : null;

    }

    /// <summary>
    /// Removes a NeosVR credential from the Windows Vault based on a given key
    /// </summary>
    /// <param name="key"></param>
    /// <returns>True if the credential was removed, false if the credentail with assigned key does not exist</returns>
    public bool RemoveCredential(string key) => CredDelete(CreateNeosKey(key), CredType.GENERIC, 0);

    [DllImport("Advapi32.dll", EntryPoint = "CredReadW", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool CredRead(string target, CredType type, int reservedFlag, out IntPtr CredentialPtr);

    [DllImport("Advapi32.dll", SetLastError = true, EntryPoint = "CredWriteW", CharSet = CharSet.Unicode)]
    private static extern bool CredWrite([In] ref NativeVaultCredential userCredential, [In] uint flags);

    [DllImport("Advapi32.dll", EntryPoint = "CredDeleteW", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool CredDelete(string target, CredType type, int reservedFlag);

    private static string CreateNeosKey(string key) => $"NeosVR:{key}";
}