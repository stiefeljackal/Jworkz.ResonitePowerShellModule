using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JworkzNeosPowerShellModule.Utilities;

public static class StringExtensions
{
    /// <summary>
    /// Regex that matches query param strings as found in URLs
    /// </summary>
    public static readonly Regex QUERY_PARAM_REGEX = new Regex(@"(?<key>[\w_-]+)=(?<value>[\w_-]+)");

    /// <summary>
    /// Converts string to a SecureString object
    /// </summary>
    /// <param name="str">String to convert</param>
    /// <returns>String as a SecureString object</returns>
    public static SecureString ToSecureString(this string str)
    {
        var secureStr = new SecureString();
        var chars = str.ToCharArray();

        foreach (char c in chars)
        {
            secureStr.AppendChar(c);
        }

        return secureStr;
    }

    /// <summary>
    /// Converts string to a byte[]
    /// </summary>
    /// <param name="str">String to convert</param>
    /// <returns>String as a byte[]</returns>
    public static byte[]? ToBytes(this string str) => str == null ? null : Encoding.Unicode.GetBytes(str);

    /// <summary>
    /// Converts SecureString object to a byte[]
    /// </summary>
    /// <param name="secureStr">SecureString to convert</param>
    /// <returns>SecureString as a byte[]</returns>
    public static byte[]? ToBytes(this SecureString secureStr) => secureStr.ToPlainText().ToBytes();

    /// <summary>
    /// Converts SecureString object to plain text that is readable
    /// </summary>
    /// <param name="secureStr">SecureString object to convert to plain text</param>
    /// <returns>SecureString as plain text</returns>
    public static string ToPlainText(this SecureString secureStr) =>
        Marshal.PtrToStringAuto(Marshal.SecureStringToBSTR(secureStr)) ?? string.Empty;

    /// <summary>
    /// Converts a byte array into a hash string
    /// </summary>
    /// <param name="bytes">Byte array to generate the hash string from</param>
    /// <param name="upperCase">To use uppercase letters or not</param>
    /// <returns>byte[] as a hash string</returns>
    public static string ToHex(this byte[] bytes, bool upperCase = false)
    {
        var sb = new StringBuilder(bytes.Length * 2);

        for (int i = 0; i < bytes.Length; i++)
        {
            sb.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));
        }

        return sb.ToString();
    }
}
