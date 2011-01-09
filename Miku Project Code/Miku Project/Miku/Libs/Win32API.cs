using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Miku.Client
{
    /// <summary>
    /// The helper class to access the windows api.
    /// </summary>
    public class Win32API
    {

        /// <summary>
        /// The FindWindow function retrieves a handle to the top-level window whose class name and 
        /// window name match the specified strings. This function does not search child windows. This function does not perform a case-sensitive search.
        /// </summary>
        /// <param name="lpClassName">
        ///  Pointer to a null-terminated string that specifies the class name or a class atom created by a previous call to the RegisterClass or RegisterClassEx function. 
        ///  The atom must be in the low-order word of lpClassName; the high-order word must be zero. 
        ///  If lpClassName points to a string, it specifies the window class name. The class name can be any name registered with RegisterClass or RegisterClassEx, or any of the predefined control-class names. 
        ///  If lpClassName is NULL, it finds any window whose title matches the lpWindowName parameter </param>
        /// <param name="lpWindowName">Pointer to a null-terminated string that specifies the window name (the window's title). If this parameter is NULL, all window names match.</param>
        /// <returns>If the function succeeds, the return value is a handle to the window that has the specified class name and window name.
        /// If the function fails, the return value is NULL. To get extended error information, call GetLastError. </returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// The FindWindowEx function retrieves a handle to a window whose class name and window name match the specified strings. The function searches child windows, 
        /// beginning with the one following the specified child window. This function does not perform a case-sensitive search. 
        /// </summary>
        /// <param name="hwndParent">Handle to the parent window whose child windows are to be searched. </param>
        /// <param name="hwndChildAfter">Handle to a child window. The </param>
        /// <param name="lpszClass">Pointer to a null-terminated string that specifies the class name or a class atom created by a previous call to the RegisterClass or RegisterClassEx function. 
        /// The atom must be placed in the low-order word of lpszClass; the high-order word must be zero</param>
        /// <param name="lpszWindow"> Pointer to a null-terminated string that specifies the window name (the window's title). </param>
        /// <returns>If the function succeeds, the return value is a handle to the window that has the specified class and window names.
        /// If the function fails, the return value is NULL. To get extended error information, call GetLastError. </returns>
        [DllImport("user32.dll")]
        public extern static IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        /// <summary>
        /// Updates the client area.
        /// </summary>
        /// <param name="hWndParent"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public extern static IntPtr UpdateWindow(IntPtr hWndParent);

        /// <summary>
        /// The SendMessage function sends the specified message to a window or windows. It calls the window procedure for the specified window and does not return until the window procedure has 
        /// processed the message. 
        /// </summary>
        /// <param name="hWnd">Handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST, the message is sent to all top-level windows in the system, 
        /// including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.
        /// Microsoft Windows Vista and later. Message sending is subject to User Interface Privilege Isolation (UIPI). The thread of a process can send messages only to message queues of threads 
        /// in processes of lesser or equal integrity level.</param>
        /// <param name="Msg">Specifies the message to be sent.</param>
        /// <param name="wParam">Specifies additional message-specific information.</param>
        /// <param name="lParam"> Specifies additional message-specific information.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        [DllImport("user32.DLL")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="message"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hwnd, uint Msg, int wParam, int lParam);

        /// <summary>
        /// The EnumWindows function enumerates all top-level windows on the screen by passing the handle to each window, in turn, to an application-defined callback function. EnumWindows continues
        /// until the last top-level window is enumerated or the callback function returns FALSE.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern int EnumWindows(CallBack callback, ref bool lParam);
        public delegate bool CallBack(IntPtr hwnd, ref bool lParam);


        [DllImport("user32.dll")]
        public extern static int EnumChildWindows(IntPtr hWndParent, CallBack lpEnumFunc, ref bool lParam);

        [DllImport("user32.dll")]
        public extern static int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        public extern static int GetClassName(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        public enum HookType
        {
            WH_JOURNALRECORD = 0,
            WH_JOURNALPLAYBACK = 1,
            WH_KEYBOARD = 2,
            WH_GETMESSAGE = 3,
            WH_CALLWNDPROC = 4,
            WH_CBT = 5,
            WH_SYSMSGFILTER = 6,
            WH_MOUSE = 7,
            WH_HARDWARE = 8,
            WH_DEBUG = 9,
            WH_SHELL = 10,
            WH_FOREGROUNDIDLE = 11,
            WH_CALLWNDPROCRET = 12,
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14,
            WH_MSGFILTER = -1,
        }

        #region Hook And Simulate
        public delegate IntPtr HookProc(int code, int wParam, IntPtr lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowsHookEx(HookType hookType, HookProc hook, IntPtr instance, int threadID);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr CallNextHookEx(IntPtr hookHandle, int code, int wParam, IntPtr lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool UnhookWindowsHookEx(IntPtr hookHandle);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bVk">模拟键盘的键码</param>
        /// <param name="vScan">按键的OEM扫描码</param>
        /// <param name="dwFlags">0或者是KEYEVENT_KEYUP(0X02)</param>
        /// <param name="dwExtraInfo">通常不用，设为0</param>
        [DllImport("User32.dll", EntryPoint = "keybd_event")]
        public static extern void Keybd_Event(int bVk, int vScan, KBEventFlag dwFlags, int dwExtraInfo);

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

        [DllImport("User32.dll", EntryPoint = "mouse_event")]
        public static extern void Mouse_Event(MouseEventFlag flag, int dx, int dy, uint data, UIntPtr extraInfo);

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

        [DllImport("user32.dll")]
        public extern static bool SetCursorPos(int x, int y);

        #endregion

        #region HotKey

        [DllImport("user32.dll")]
        public static extern UInt32 RegisterHotKey(IntPtr hWnd, UInt32 id, UInt32 fsModifiers, UInt32 vk);

        [DllImport("user32")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        #endregion

        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        public static extern bool BitBlt(
            IntPtr hdcDest,
            int nXDest,
            int nYDest,
            int nWidth,
            int nHeight,
            IntPtr hdcSrc,
            int nXSrc,
            int nYSrc,
            System.Int32 dwRop
        );

        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        public static extern IntPtr CreateDC(
            string lpszDriver,
            string lpszDevice,
            string lpszOutput,
            IntPtr lpInitData
        );
    }


}
