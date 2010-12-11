using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Reflection;

namespace Miku.Client.Models.Hooks
{
    public enum KeyboardEvents
    {
        WM_KeyDown = 0x0100,
        WM_KeyUp = 0x0101,
        WM_SystemKeyDown = 0x0104,
        WM_SystemKeyUp = 0x0105
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct KBDLLHOOKSTRUCT 
    {
        public int vkCode; //Specifies a virtual-key code. The code must be a value in the range 1 to 254. 
        public int scanCode; //Specifies a hardware scan code for the key.
        public int flags;
        public int time;//Specifies the time stamp for this message
        public int dwExtraInfo;
    }

    public delegate void KeyboardEventHandler(KeyboardEvents keyEvent, System.Windows.Forms.Keys keyData,int time);

    public class KBDLLHook : Hook
    {
        public event KeyboardEventHandler KeyboardEvent;

        public KBDLLHook(IntPtr hookedInstance)
            : base(hookedInstance)
        {
            hookProcEx = new Win32API.HookProc(HookProc);
        }

        public bool SetHook()
        {
            return base.SetHook(Win32API.HookType.WH_KEYBOARD_LL);
        }

        public override IntPtr HookProc(int code, int wParam, IntPtr lParam)
        {
            if (code >= 0)
            {
                KeyboardEvents kEvent = (KeyboardEvents)wParam;
                if (kEvent != KeyboardEvents.WM_KeyDown &&
                        kEvent != KeyboardEvents.WM_KeyUp &&
                        kEvent != KeyboardEvents.WM_SystemKeyDown &&
                        kEvent != KeyboardEvents.WM_SystemKeyUp)
                {
                    return Win32API.CallNextHookEx(this.hookHandle, (int)Win32API.HookType.WH_KEYBOARD_LL, wParam, lParam);
                }
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
                    KeyboardEvent(kEvent, keyData, MyKey.time-previousHookTimestamp);
                    previousHookTimestamp = MyKey.time;
                }
            }

            //把钩子信息传递给钩子链中下一个等待接收信息的钩子过程
            return Win32API.CallNextHookEx(this.hookHandle, (int)Win32API.HookType.WH_KEYBOARD_LL, wParam, lParam);
        }
    }
}
