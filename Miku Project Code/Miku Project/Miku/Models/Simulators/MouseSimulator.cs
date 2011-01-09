using System;
using System.Collections.Generic;
using System.Text;

namespace Miku.Client.Models.Simulators
{
    /// <summary>
    /// The simulator that simulates the mouse event
    /// </summary>
    public class MouseSimulator
    {
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
