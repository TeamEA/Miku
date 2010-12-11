using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows.Forms;

namespace Miku.Client.Models.Hooks
{
    /// <summary>
    /// The hook that hooks the messages
    /// </summary>
    public abstract class Hook
    {
        protected IntPtr hookedInstance;
        protected IntPtr hookHandle;
        protected int threadID;
        protected Win32API.HookProc hookProcEx;

        //fields to calculate the delaytime
        protected bool firstHook;
        protected int previousHookTimestamp;

        /// <summary>
        /// The construct function
        /// </summary>
        /// <param name="hookedInstance">the instance to be hooked on</param>
        public Hook(IntPtr hookedInstance)
        {
            this.hookedInstance = hookedInstance;
            this.threadID = 0;
            hookHandle = IntPtr.Zero;

            firstHook = true;
            previousHookTimestamp = 0;
        }

        /// <summary>
        /// Sets the hook.
        /// </summary>
        /// <param name="hookType">Type of the hook.</param>
        /// <returns>if hook successfully then return true,otherwise return false</returns>
        protected bool SetHook(Win32API.HookType hookType)
        {
            this.hookHandle = Win32API.SetWindowsHookEx(hookType, hookProcEx, this.hookedInstance, this.threadID);
            return (hookHandle != IntPtr.Zero);
        }

        /// <summary>
        /// The hook procedure,when hook the message,take some actions in this method
        /// </summary>
        /// <param name="code"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public virtual IntPtr HookProc(int code, int wParam, IntPtr lParam)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// unhook.
        /// </summary>
        /// <returns>if unhook successfully then return true,otherwise return false</returns>
        public virtual bool UnHook()
        {
            return Win32API.UnhookWindowsHookEx(this.hookHandle);
        }
    }
}
