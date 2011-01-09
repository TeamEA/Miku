using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Miku.Client.Models.Hooks
{
    public class KBMLLHook
    {
        public event KeyboardEventHandler KeyboardEvent;
        public event MouseEventHandler MouseEvent;

        private IntPtr instance;
        private IntPtr kbHookHandle;
        private IntPtr mHookHandle;
        private int threadID;
        private Win32API.HookProc kbHookProcEx;
        private Win32API.HookProc mHookProcEx;

        private bool firstHook;
        private int previousHookTimestamp;

        private Win32API.POINT previousMousePosition;

        public KBMLLHook(IntPtr hookedInstance)
        {
            this.instance = hookedInstance;
            this.threadID = 0;
            kbHookHandle = IntPtr.Zero;
            mHookHandle = IntPtr.Zero;

            firstHook = true;
            previousHookTimestamp = 0;

            kbHookProcEx = new Win32API.HookProc(KBHookProc);
            mHookProcEx = new Win32API.HookProc(MHookProc);

            previousMousePosition = new Win32API.POINT(-1, -1);
        }

        public bool SetHook()
        {
            this.kbHookHandle = Win32API.SetWindowsHookEx(Win32API.HookType.WH_KEYBOARD_LL, kbHookProcEx, this.instance, this.threadID);
            this.mHookHandle = Win32API.SetWindowsHookEx(Win32API.HookType.WH_MOUSE_LL, mHookProcEx, this.instance, this.threadID);
            return (kbHookHandle != IntPtr.Zero && mHookHandle != IntPtr.Zero);
        }

        public virtual bool UnHook()
        {
            return Win32API.UnhookWindowsHookEx(this.mHookHandle) &&
                Win32API.UnhookWindowsHookEx(this.kbHookHandle);
        }

        public IntPtr KBHookProc(int code, int wParam, IntPtr lParam)
        {
            if (code >= 0)
            {
                KeyboardEvents kEvent = (KeyboardEvents)wParam;

                if (kEvent == KeyboardEvents.WM_KeyDown ||
                        kEvent == KeyboardEvents.WM_KeyUp ||
                        kEvent == KeyboardEvents.WM_SystemKeyDown ||
                        kEvent == KeyboardEvents.WM_SystemKeyUp)
                {
                    KBDLLHOOKSTRUCT MyKey = new KBDLLHOOKSTRUCT();
                    Type t = MyKey.GetType();
                    MyKey = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, t);
                    //int vkCode = Marshal.ReadInt32(lParam);
                    Keys keyData = (Keys)MyKey.vkCode;

                    if (firstHook == true)
                    {
                        KeyboardEvent(kEvent, keyData, 0);
                        previousHookTimestamp = MyKey.time;
                        firstHook = false;
                    }
                    else
                    {
                        KeyboardEvent(kEvent, keyData, MyKey.time - previousHookTimestamp);
                        previousHookTimestamp = MyKey.time;
                    }

                }
            }
            //把钩子信息传递给钩子链中下一个等待接收信息的钩子过程
            return Win32API.CallNextHookEx(this.kbHookHandle, (int)Win32API.HookType.WH_KEYBOARD_LL, wParam, lParam);
        }

        public IntPtr MHookProc(int code, int wParam, IntPtr lParam)
        {
            if (code >= 0)
            {
                MouseEvents mouseEvent = (MouseEvents)wParam;

                if (mouseEvent == MouseEvents.WM_LBUTTONDBLCLK ||
                        mouseEvent == MouseEvents.WM_LBUTTONDOWN ||
                        mouseEvent == MouseEvents.WM_LBUTTONUP ||
                        mouseEvent == MouseEvents.WM_MBUTTONDBLCLK ||
                        mouseEvent == MouseEvents.WM_MBUTTONDOWN ||
                        mouseEvent == MouseEvents.WM_MBUTTONUP ||
                        mouseEvent == MouseEvents.WM_MOUSEMOVE ||
                        mouseEvent == MouseEvents.WM_MOUSEWHEEL ||
                        mouseEvent == MouseEvents.WM_RBUTTONDBLCLK ||
                        mouseEvent == MouseEvents.WM_RBUTTONDOWN ||
                        mouseEvent == MouseEvents.WM_RBUTTONUP)
                {
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
                        if (mouseEvent == MouseEvents.WM_MOUSEMOVE )
                        {
                            if (previousMousePosition.Equals( MyMouse.pt))
                            {
                                //把钩子信息传递给钩子链中下一个等待接收信息的钩子过程
                                return Win32API.CallNextHookEx(this.mHookHandle, (int)Win32API.HookType.WH_MOUSE_LL, wParam, lParam);
                            }

                            previousMousePosition = MyMouse.pt;
                        }

                        MouseEvent(mouseEvent, MyMouse, MyMouse.time - previousHookTimestamp);

                        previousHookTimestamp = MyMouse.time;
                    }
                }
            }


            //把钩子信息传递给钩子链中下一个等待接收信息的钩子过程
            return Win32API.CallNextHookEx(this.mHookHandle, (int)Win32API.HookType.WH_MOUSE_LL, wParam, lParam);
        }
    }
}
