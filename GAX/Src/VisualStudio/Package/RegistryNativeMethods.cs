using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Practices.RecipeFramework.VisualStudio
{
    public static class RegistryNativeMethods
    {
        [Flags]
        public enum RegSAM
        {
            AllAccess = 0x000f003f,
            KeyRead = 0x00020019
        }

        private const int REG_PROCESS_APPKEY = 0x00000001;

        // approximated from pinvoke.net's RegLoadKey and RegOpenKey
        // NOTE: changed return from long to int so we could do Win32Exception on it
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int RegLoadAppKey(String hiveFile, out int hKey, RegSAM samDesired, int options, int reserved);

        public static int RegLoadAppKey(String hiveFile)
        {
            int hKey;
            int rc = RegLoadAppKey(hiveFile, out hKey, RegSAM.KeyRead, REG_PROCESS_APPKEY, 0);

            if (rc != 0)
            {
                throw new Win32Exception(rc, "Failed during RegLoadAppKey of file " + hiveFile);
            }

            return hKey;
        }
        static SafeRegistryHandle safeRegistryHandle = null;
        public static RegistryKey OpenSubKey(String entry)
        {
            RegistryKey rrk = null;
            if (safeRegistryHandle == null)
            {
                //TODO: approfondore errore di condivisione in aprtura del privateregistry.bin
                int _hKey = RegistryNativeMethods.RegLoadAppKey(@"C:\Temp\privateregistry.bin");
                //int _hKey = RegistryNativeMethods.RegLoadAppKey(Environment.ExpandEnvironmentVariables("%localappdata%")+@"\Microsoft\VisualStudio\15.0_" + RecipeManager.GetCurrentVsHive() + @"\privateregistry.bin");
                //TODO: approfondore errore di condivisione in aprtura del privateregistry.bin
                safeRegistryHandle = new SafeRegistryHandle(new IntPtr(_hKey), true);
            }
            if (safeRegistryHandle != null)
            { 
                RegistryKey appKey = RegistryKey.FromHandle(safeRegistryHandle);
                rrk = appKey.OpenSubKey(entry);
            }
            return rrk;
        }
    }
}
