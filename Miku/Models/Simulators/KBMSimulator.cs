using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miku.Client.Models.Simulators
{
    /// <summary>
    /// The simulator that simulates the keyboard and mouse event 
    /// </summary>
    public class KBMSimulator
    {
        public KBMSimulator()
        {
        }

        /// <summary>
        /// Simulate the keyboard event
        /// </summary>
        /// <param name="bVk">Specifies a virtual-key code. The code must be a value in the range 1 to 254.</param>
        /// <param name="vScan">Specifies a hardware scan code for the key.</param>
        /// <param name="dwFlags"></param>
        /// <param name="dwExtraInfo"></param>
        public void Simulate(int bVk, int vScan, Win32API.KBEventFlag dwFlags, int dwExtraInfo)
        {
            Win32API.Keybd_Event(bVk, vScan, dwFlags, dwExtraInfo);
        }

        /// <summary>
        /// Simulate the mouse event
        /// </summary>
        /// <param name="flag"> the event flag</param>
        /// <param name="dx">the cursor position on the x coordinate</param>
        /// <param name="dy">the cursor position on the y coordinate</param>
        /// <param name="data"></param>
        /// <param name="extraInfo"></param>
        public void Simulate(Win32API.MouseEventFlag flag, int dx, int dy, uint data, UIntPtr extraInfo)
        {
            Win32API.SetCursorPos(dx, dy);//set the cursor to the position

            //if the mouseeventflag contains move flag ,we just ignore it ,because we have already set the cursor to the position
            if ((flag & Win32API.MouseEventFlag.Move) == 0)
            {
                Win32API.Mouse_Event(flag | Win32API.MouseEventFlag.Absolute, dx, dy, data, extraInfo);
            }
        }
    }
}
