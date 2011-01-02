using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Miku.Server
{
    /// <summary>
    /// The helper class to access the windows api.
    /// </summary>
    public class Win32API
    {

        public enum KBEventFlag : uint
        {
            KeyUp = 0x2,
            KeyDown = 0x0
        }

        [Serializable]
        public struct KeyEvent
        {
            public int bVk;
            public int vScan;
            public KBEventFlag dwFlags;
            public int delayTime;
            public int dwExtraInfo;
        }

        public enum MouseEventFlag : uint
        {
            Move = 0x0001,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            XDown = 0x0080,
            XUp = 0x0100,
            Wheel = 0x0800,
            VirtualDesk = 0x4000,
            Absolute = 0x8000
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;

            public POINT(int x = 0, int y = 0)
            {
                this.x = x;
                this.y = y;
            }
            public override string ToString()
            {
                return string.Format("({0},{1})", x.ToString(), y.ToString());
            }
        }

        [Serializable]
        public struct MouseEvent
        {
            public POINT pt;
            public MouseEventFlag dwFlags;
            public int delayTime;
            public UIntPtr dwExtraInfo;
        }

    }


}
