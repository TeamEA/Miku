using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;

namespace Miku.Client.Models.Hooks
{
    public enum MouseEvents
    {
        WM_MOUSEMOVE = 0x0200,
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_LBUTTONDBLCLK = 0x0203, 
        WM_RBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205,
        WM_RBUTTONDBLCLK = 0x0206,
        WM_MBUTTONDOWN = 0x0207,
        WM_MBUTTONUP = 0x0208,
        WM_MBUTTONDBLCLK = 0x0209,
        WM_MOUSEWHEEL = 0x020A,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MSLLHOOKSTRUCT 
    {
        public Win32API.POINT pt; 
        public int mouseData;
        public int flag;
        public int time;
        public int dwExtraInfo;
    }

    public delegate void MouseEventHandler(MouseEvents mouseEvent,MSLLHOOKSTRUCT mouse,int delayTime);

    public class MSLLHook : Hook
    {
        public event MouseEventHandler MouseEvent;

        public MSLLHook(IntPtr hookedInstance)
            : base(hookedInstance)
        {
            hookProcEx = new Win32API.HookProc(HookProc);
        }

        public bool SetHook()
        {
            return base.SetHook(Win32API.HookType.WH_MOUSE_LL);
        }

        public override IntPtr HookProc(int code, int wParam, IntPtr lParam)
        {
            if (code >= 0)
            {
                MouseEvents mouseEvent = (MouseEvents)wParam;

                if (mouseEvent != MouseEvents.WM_LBUTTONDBLCLK &&
                        mouseEvent != MouseEvents.WM_LBUTTONDOWN &&
                        mouseEvent != MouseEvents.WM_LBUTTONUP &&
                        mouseEvent != MouseEvents.WM_MBUTTONDBLCLK &&
                        mouseEvent != MouseEvents.WM_MBUTTONDOWN &&
                        mouseEvent != MouseEvents.WM_MBUTTONUP &&
                        mouseEvent != MouseEvents.WM_MOUSEMOVE &&
                        mouseEvent != MouseEvents.WM_MOUSEWHEEL &&
                        mouseEvent != MouseEvents.WM_RBUTTONDBLCLK &&
                        mouseEvent != MouseEvents.WM_RBUTTONDOWN &&
                        mouseEvent != MouseEvents.WM_RBUTTONUP)
                {
                    return Win32API.CallNextHookEx(this.hookHandle, (int)Win32API.HookType.WH_KEYBOARD_LL, wParam, lParam);
                }

                MSLLHOOKSTRUCT MyMouse = new MSLLHOOKSTRUCT();
                Type t = MyMouse.GetType();
                MyMouse = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, t);

                if (firstHook == true)
                {
                    MouseEvent(mouseEvent, MyMouse, 0);
                    previousHookTimestamp = MyMouse.time;
                    firstHook = false;
                }
                else
                {
                    MouseEvent(mouseEvent, MyMouse, MyMouse.time - previousHookTimestamp);
                    previousHookTimestamp = MyMouse.time;
                }
            }

            //把钩子信息传递给钩子链中下一个等待接收信息的钩子过程
            return Win32API.CallNextHookEx(this.hookHandle, (int)Win32API.HookType.WH_MOUSE_LL, wParam, lParam);
        }
    }
}
