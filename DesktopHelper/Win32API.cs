using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace DesktopHelper
{
    /// <summary>
    /// The helper class to access the windows api.
    /// </summary>
    class Win32API
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
    }
}
