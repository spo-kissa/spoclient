﻿using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace SpoClient.Setting
{
    public static class SecureStringExtensions
    {
        public static string? ToUnsecureString(this SecureString secureString)
        {
            ArgumentNullException.ThrowIfNull(secureString);

            var unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                if (unmanagedString != IntPtr.Zero)
                {
                    Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
                }
            }
        }


        public static byte[] ToUnsecureBytes(this SecureString secureString)
        {
            var unsecureString = ToUnsecureString(secureString);
            if (unsecureString is null)
            {
                return [];
            }

            return Encoding.UTF8.GetBytes(unsecureString);
        }


        public static SecureString FromString(this SecureString secureString, string source)
        {
            ArgumentNullException.ThrowIfNull(secureString);
            ArgumentNullException.ThrowIfNull(source);

            foreach (char c in source)
            {
                secureString.AppendChar(c);
            }

            return secureString;
        }


        public static SecureString ToSecureString(this string source)
        {
            ArgumentNullException.ThrowIfNull(source);

            var secureString = new SecureString();
            foreach (char c in source)
            {
                secureString.AppendChar(c);
            }
            return secureString;
        }
    }
}
