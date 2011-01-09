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
        /// <summary>
        /// Registries to realize auto run.
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        public static void RegistryAutoRun(string filepath)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            key.SetValue("", filepath);
            key.Close();
        }

        /// <summary>
        /// Reigstries the current module to realize auto run.
        /// </summary>
        public static void ReigstryCurrentModuleAutoRun()
        {
            RegistryAutoRun(Application.ExecutablePath);
        }
    }
}
