using System;
using System.Collections.Generic;
using System.Text;

namespace Miku.Client.Models.Simulators
{
    /// <summary>
    /// The simulator that simulates the keyboard event
    /// </summary>
    public class KeyboardSimulator
    {
        /// <summary>
        /// Simulate the keyboard event
        /// </summary>
        /// <param name="bVk">Specifies a virtual-key code. The code must be a value in the range 1 to 254.</param>
        /// <param name="vScan">Specifies a hardware scan code for the key.</param>
        /// <param name="dwFlags"></param>
        /// <param name="dwExtraInfo"></param>
        public void Simulate(int bVk, int vScan, Win32API.KBEventFlag dwFlags, int dwExtraInfo)
        {
             Win32API.Keybd_Event( bVk, vScan, dwFlags, dwExtraInfo);
        }
    }
}
