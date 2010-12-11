using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Windows.Forms;

namespace Miku.Client.Helpers
{
    public static class RegistryHelper
    {
        public static void RegistryAutoRun(string filepath)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            key.SetValue("", filepath);
            key.Close();
        }

        public static void ReigstryCurrentModuleAutoRun()
        {
            RegistryAutoRun(Application.ExecutablePath);
        }
    }
}
